using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Exceptions;

namespace ZmitaCart.Infrastructure.Repositories;

public class GoogleAuthentication : IGoogleAuthentication
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly GoogleSettings _googleSettings;

	public GoogleAuthentication(IOptions<GoogleSettings> googleSettings, UserManager<User> userManager, 
		SignInManager<User> signInManager, IJwtTokenGenerator jwtTokenGenerator)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_jwtTokenGenerator = jwtTokenGenerator;
		_googleSettings = googleSettings.Value;
	}

	public async Task<string> AuthenticateAsync(ExternalAuthDto externalAuthDto)
	{
		var payload = await VerifyTokenAsync(externalAuthDto.Token);

		if (payload == null)
			throw new InvalidDataException("Failed to authenticate user");

		var user = await _userManager.FindByEmailAsync(payload.Email);
		
		if (user == null)
			await RegisterAsync(payload);
		
		return await LoginAsync(payload);
	}

	private async Task<GoogleJsonWebSignature.Payload> VerifyTokenAsync(string token)
	{
		var settings = new GoogleJsonWebSignature.ValidationSettings
		{
			Audience = new List<string>{ _googleSettings.ClientId },
		};
		
		var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
		
		if (payload == null)
			throw new InvalidDataException("Invalid token");

		return payload;
	}
	
	private async Task RegisterAsync(GoogleJsonWebSignature.Payload payload)
	{
		var user = new User
		{
			Email = payload.Email,
			UserName = payload.Email,
			FirstName = payload.GivenName,
			LastName = payload.FamilyName,
			EmailConfirmed = true
		};

		var result = await _userManager.CreateAsync(user);

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
	
	private async Task<string> LoginAsync(GoogleJsonWebSignature.Payload payload)
	{
		var user = await _userManager.FindByEmailAsync(payload.Email);

		if (user == null)
			throw new InvalidDataException("Failed to authenticate user");
		
		await _signInManager.SignInAsync(user, true, "Google");

		var authClaims = await _userManager.GetClaimsAsync(user);

		return _jwtTokenGenerator.CreateToken(authClaims);
	}
}