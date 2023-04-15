using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly IMapper _mapper;
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;

	public UserRepository(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, 
		RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator)
	{
		_mapper = mapper;
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_jwtTokenGenerator = jwtTokenGenerator;
	}

	public async Task RegisterAsync(RegisterUserDto registerUserDto)
	{
		if (await _userManager.FindByEmailAsync(registerUserDto.Email) != null)
		{
			throw new Exception("User already exists");
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
			var errors = result.Errors.Select(e => e.Description).ToString();
			throw new InvalidDataException(errors);
		}
		
		if (!await _roleManager.RoleExistsAsync(registerUserDto.Role?.Code))
		{
			throw new InvalidDataException("Unsupported role");
		}
		
		await _userManager.AddToRoleAsync(user, registerUserDto.Role?.Code);
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
		
		if(!signInResult.Succeeded)
		{
			throw new InvalidDataException("Could not sign in");
		}
		
		if (signInResult.IsLockedOut)
		{
			throw new Exception("User is locked out");
		}
		
		var authClaims = new List<Claim>
		{
			new(ClaimTypes.Sid, user.Id.ToString()),
			new(ClaimTypes.Email, user.Email),
			new(ClaimTypes.GivenName, user.FirstName),
			new(ClaimTypes.Surname, user.LastName)
		};

		var userRoles = await _userManager.GetRolesAsync(user);
		authClaims.AddRange
		(
			userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole))
		);

		return _jwtTokenGenerator.CreateToken(authClaims);
	}

	public async Task LogoutAsync()
	{
		await _signInManager.SignOutAsync();
	}
	
	public async Task AddRoleAsync(string userEmail, Role newRole)
	{
		throw new NotImplementedException();
	}
}