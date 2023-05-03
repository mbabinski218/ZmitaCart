using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Domain.ValueObjects;
using ZmitaCart.Infrastructure.Exceptions;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IGoogleAuthentication _googleAuthentication;
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager,
		RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator, IGoogleAuthentication googleAuthentication,
		ApplicationDbContext dbContext, IMapper mapper)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_jwtTokenGenerator = jwtTokenGenerator;
		_googleAuthentication = googleAuthentication;
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task RegisterAsync(RegisterUserDto registerUserDto)
	{
		if (await _userManager.FindByEmailAsync(registerUserDto.Email) != null)
		{
			throw new InvalidDataException("User already exists");
		}

		var user = new User
		{
			Email = registerUserDto.Email,
			UserName = registerUserDto.Email,
			FirstName = registerUserDto.FirstName,
			LastName = registerUserDto.LastName,
			EmailConfirmed = true
		};

		var result = await _userManager.CreateAsync(user, registerUserDto.Password);

		if (!result.Succeeded)
		{
			throw new InvalidLoginDataException(result.Errors.Select(e => e.Description));
		}

		await _userManager.AddToRoleAsync(user, Role.user);

		var claims = new List<Claim>
		{
			new(ClaimNames.Id, user.Id.ToString()),
			new(ClaimNames.Email, user.Email),
			new(ClaimNames.FirstName, user.FirstName),
			new(ClaimNames.LastName, user.LastName),
			new(ClaimNames.Role, Role.user)
		};

		await _userManager.AddClaimsAsync(user, claims);
	}

	public async Task<string> LoginAsync(LoginUserDto loginUserDto)
	{
		var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

		if (user == null)
		{
			throw new InvalidDataException("User does not exist");
		}

		if (!await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
		{
			throw new InvalidDataException("Invalid password");
		}

		var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password,
			false, true);

		if (!signInResult.Succeeded)
		{
			throw new InvalidDataException("Could not sign in");
		}

		if (signInResult.IsLockedOut)
		{
			throw new Exception("User is locked out");
		}

		var authClaims = await _userManager.GetClaimsAsync(user);

		return _jwtTokenGenerator.CreateToken(authClaims);
	}

	public async Task LogoutAsync()
	{
		await _signInManager.SignOutAsync();
	}

	public async Task AddRoleAsync(string userEmail, string role)
	{
		var user = await _userManager.FindByEmailAsync(userEmail);

		if (user == null)
		{
			throw new InvalidDataException("User does not exist");
		}

		if (!await _roleManager.RoleExistsAsync(role))
		{
			throw new InvalidDataException("Unsupported role");
		}

		await _userManager.AddToRoleAsync(user, role);

		await _userManager.AddClaimAsync(user, new Claim(ClaimNames.Role, role));
	}

	public async Task UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user is null)
		{
			throw new InvalidDataException("User does not exist");
		}

		if (phoneNumber is not null)
		{
			var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);

			if (!result.Succeeded)
			{
				throw new InvalidDataException("Could not set phone number");
			}
		}

		if (address is not null && address != user.Address)
		{
			user.Address = address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				throw new InvalidDataException("Could not set address");
			}
		}
	}

	public async Task<string> ExternalAuthenticationAsync(ExternalAuthDto externalAuthDto)
	{
		return externalAuthDto.Provider switch
		{
			"Google" => await _googleAuthentication.AuthenticateAsync(externalAuthDto),
			_ => throw new InvalidDataException("Failed to authenticate")
		};
	}


	public async Task<int> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment)
	{
		var rater = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == raterId)
		            ?? throw new NotFoundException("Rater does not exist");

		var feedback = new Feedback
		{
			RecipientId = recipientId,
			RaterId = raterId,
			Rater = rater,
			Rating = rating,
			Comment = comment
		};

		await _dbContext.Feedbacks.AddAsync(feedback);
		await _dbContext.SaveChangesAsync();
		return feedback.Id;
	}

	public async Task<int> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId)
		               ?? throw new NotFoundException("Feedback does not exist");
		
		if (feedback.RaterId != raterId) throw new UnauthorizedAccessException("You are not allowed to update this feedback");

		feedback.Rating = rating ?? feedback.Rating;
		feedback.Comment = comment ?? feedback.Comment;

		await _dbContext.SaveChangesAsync();
		return feedback.Id;
	}

	public async Task DeleteFeedbackAsync(int feedbackId)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId)
		               ?? throw new NotFoundException("Feedback does not exist");

		_dbContext.Feedbacks.Remove(feedback);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<PaginatedList<FeedbackDto>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize)
	{
		return await _dbContext.Feedbacks
			.Where(f => f.RecipientId == userId)
			.Include(f => f.Rater)
			.ProjectTo<FeedbackDto>(_mapper.ConfigurationProvider)
			.ToPaginatedListAsync(pageNumber, pageSize);
	}
}