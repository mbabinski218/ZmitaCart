using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Exceptions;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly RoleManager<IdentityRole<int>> _roleManager;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;

	public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, 
		RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_jwtTokenGenerator = jwtTokenGenerator;
	}

	public async Task RegisterAsync(RegisterUserDto registerUserDto)
	{
		if (await _userManager.FindByEmailAsync(registerUserDto.Email) != null)
		{
			throw new InvalidDataException("User already exists");
		}

		if (registerUserDto.Role == null || !await _roleManager.RoleExistsAsync(registerUserDto.Role.Code))
		{
			throw new InvalidDataException("Unsupported role");
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

		await _userManager.AddToRoleAsync(user, registerUserDto.Role!.Code);

		var claims = new List<Claim>
		{
			new(ClaimNames.Id, user.Id.ToString()),
			new(ClaimNames.Email, user.Email),
			new(ClaimNames.FirstName, user.FirstName),
			new(ClaimNames.LastName, user.LastName),
			new(ClaimNames.Role, registerUserDto.Role!.Code)
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
		
		if(!signInResult.Succeeded)
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
	
	public async Task AddRoleAsync(string userEmail, Role newRole)
	{
		var user = await _userManager.FindByEmailAsync(userEmail);
		
		if (user == null)
		{
			throw new InvalidDataException("User does not exist");
		}
		
		if (!await _roleManager.RoleExistsAsync(newRole.Code))
		{
			throw new InvalidDataException("Unsupported role");
		}
		
		await _userManager.AddToRoleAsync(user, newRole.Code);
		
		await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, newRole.Code));
	}
}