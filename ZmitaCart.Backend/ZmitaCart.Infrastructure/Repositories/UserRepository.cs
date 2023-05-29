using System.Security.Claims;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Domain.ValueObjects;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
	private readonly JwtHelper _jwtHelper;
	private readonly ApplicationDbContext _dbContext;
	private readonly GoogleAuthentication _googleAuthentication;

	public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager,
		RoleManager<IdentityRole<int>> roleManager, JwtHelper jwtHelper,
		ApplicationDbContext dbContext, GoogleAuthentication googleAuthentication)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_jwtHelper = jwtHelper;
		_dbContext = dbContext;
		_googleAuthentication = googleAuthentication;
	}

	public async Task<Result> RegisterAsync(RegisterUserDto dto)
	{
		if (await _userManager.FindByEmailAsync(dto.Email) != null)
		{
			return Result.Fail(new AlreadyExistsError("User already exists"));
		}

		var user = User.Create(dto.Email, dto.FirstName, dto.LastName);
		var result = await _userManager.CreateAsync(user, dto.Password);

		if (!result.Succeeded)
		{
			var reasons = result.Errors.Select(e => e.Description);
			return Result.Fail(new InvalidDataError("Invalid register data", reasons));
		}
		
		var role = dto.IsAdmin is true ? Role.administrator : Role.user;
		await _userManager.AddToRoleAsync(user, role);
		
		if (role is Role.administrator)
			await _userManager.AddToRoleAsync(user, Role.user);

		var claims = new List<Claim>
		{
			new(ClaimNames.Id, user.Id.ToString()),
			new(ClaimNames.Email, user.Email!),
			new(ClaimNames.FirstName, user.FirstName),
			new(ClaimNames.LastName, user.LastName),
			new(ClaimNames.Role, role)
		};
		await _userManager.AddClaimsAsync(user, claims);

		return Result.Ok();
	}

	public async Task<Result<TokensDto>> LoginAsync(LoginUserDto loginUserDto)
	{
		var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
		if (user == null) 
			return Result.Fail(new NotFoundError("User does not exist"));
		
		if (!await _userManager.CheckPasswordAsync(user, loginUserDto.Password)) 
			return Result.Fail(new InvalidDataError("Invalid password"));

		var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password, false, true);

		if (!signInResult.Succeeded) 
			return Result.Fail(new InvalidDataError("Could not sign in"));
		
		if (signInResult.IsLockedOut) 
			return Result.Fail(new InvalidDataError("User is locked out"));

		var authClaims = await _userManager.GetClaimsAsync(user);
		var accessToken = _jwtHelper.GenerateAccessToken(authClaims);
		var refreshToken = _jwtHelper.GenerateRefreshToken();

		await _userManager.SetAuthenticationTokenAsync(user, GrantType.password, "RefreshToken", refreshToken);
		
		return new TokensDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}

	public async Task<Result<TokensDto>> LoginWithRefreshTokenAsync(string? userId, string refreshToken)
	{
		if (userId == null) 
			return Result.Fail(new NotFoundError("User not found"));
		
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null) 
			return Result.Fail(new NotFoundError("User does not exist"));
		
		foreach(var authenticator in GrantType.SupportedGrantTypes)
		{
			var userRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, authenticator, "RefreshToken");

			if(userRefreshToken is null) continue;
			
			if (userRefreshToken != refreshToken)
			{
				await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
				return Result.Fail(new InvalidDataError("Invalid refresh token"));
			}

			await _signInManager.RefreshSignInAsync(user);

			var authClaims = await _userManager.GetClaimsAsync(user);
			var accessToken = _jwtHelper.GenerateAccessToken(authClaims);
			var newRefreshToken = _jwtHelper.GenerateRefreshToken();

			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
			await _userManager.SetAuthenticationTokenAsync(user, GrantType.refreshToken, "RefreshToken", newRefreshToken);

			return new TokensDto
			{
				AccessToken = accessToken,
				RefreshToken = newRefreshToken
			};
		}
		
		return Result.Fail(new InvalidDataError("Failed to login with refresh token"));
	}

	public Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken)
	{
		return _googleAuthentication.AuthenticateAsync(idToken);
	}

	public async Task<Result> LogoutAsync()
	{
		await _signInManager.SignOutAsync();
		return Result.Ok();
	}

	public async Task<Result> AddRoleAsync(string userEmail, string role)
	{
		var user = await _userManager.FindByEmailAsync(userEmail);
		
		if (user == null)
			return Result.Fail(new NotFoundError("User does not exist"));

		if (!await _roleManager.RoleExistsAsync(role))
			return Result.Fail(new NotFoundError("Role does not exist"));
		
		if (!await _userManager.IsInRoleAsync(user, role))
			return Result.Fail(new InvalidDataError("User already has this role"));

		await _userManager.AddToRoleAsync(user, role);
		await _userManager.AddClaimAsync(user, new Claim(ClaimNames.Role, role));

		return Result.Ok();
	}

	public async Task<Result> UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user is null)
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		if (phoneNumber is not null)
		{
			var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);

			if (!result.Succeeded)
			{
				return Result.Fail(new InvalidDataError("Could not set phone number"));
			}
		}

		if (address is not null && address != user.Address)
		{
			user.Address = address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				return Result.Fail(new InvalidDataError("Could not set address"));
			}
		}
		
		return Result.Ok();
	}

	public async Task<Result<int>> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment)
	{
		var rater = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == raterId);

		if (rater is null)
		{
			return Result.Fail(new NotFoundError("Rater does not exist"));
		}

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
		return Result.Ok(feedback.Id);
	}

	public async Task<Result<int>> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

		if (feedback is null)
		{
			return Result.Fail(new NotFoundError("Feedback does not exist"));
		}
			
		if (feedback.RaterId != raterId)
		{
			return Result.Fail(new UnauthorizedError("You are not allowed to update this feedback"));
		}
		
		feedback.Rating = rating ?? feedback.Rating;
		feedback.Comment = comment ?? feedback.Comment;

		await _dbContext.SaveChangesAsync();
		return Result.Ok(feedback.Id);
	}

	public async Task<Result> DeleteFeedbackAsync(int feedbackId)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

		if (feedback is null)
		{
			return Result.Fail(new NotFoundError("Feedback does not exist"));
		}
			
		_dbContext.Feedbacks.Remove(feedback);
		await _dbContext.SaveChangesAsync();
		return Result.Ok();
	}

	public async Task<Result<PaginatedList<FeedbackDto>>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize)
	{
		var feedback = await _dbContext.Feedbacks
			.Where(f => f.RecipientId == userId)
			.Include(f => f.Rater)
			.ProjectToType<FeedbackDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
		
		return Result.Ok(feedback);
	}
}