using System.Security.Claims;
using FluentResults;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Common.Errors;
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

	public async Task<Result<string>> AuthenticateAsync(ExternalAuthDto externalAuthDto)
	{
		var payload = await VerifyTokenAsync(externalAuthDto.Token);

		if (payload.IsFailed)
		{
			return payload.ToResult<string>();
		}
		
		var user = await _userManager.FindByEmailAsync(payload.Value.Email);

		if (user != null)
		{
			return await LoginAsync(payload.Value);
		}
		
		var register = await RegisterAsync(payload.Value);
		if (register.IsFailed)
		{
			return register.ToResult<string>();
		}

		return await LoginAsync(payload.Value);
	}

	private async Task<Result<GoogleJsonWebSignature.Payload>> VerifyTokenAsync(string token)
	{
		var settings = new GoogleJsonWebSignature.ValidationSettings
		{
			Audience = new List<string>{ _googleSettings.ClientId },
		};
		
		var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
		
		if (payload == null)
		{
			return Result.Fail(new InvalidDataError("Invalid Google token"));
		}

		return payload;
	}
	
	private async Task<Result> RegisterAsync(GoogleJsonWebSignature.Payload payload)
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
			var reasons = result.Errors.Select(e => e.Description);
			return Result.Fail(new InvalidDataError("Failed to register", reasons));
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
		return Result.Ok();
	}
	
	private async Task<Result<string>> LoginAsync(GoogleJsonWebSignature.Payload payload)
	{
		var user = await _userManager.FindByEmailAsync(payload.Email);

		if (user == null)
		{
			return Result.Fail(new InvalidDataError("Failed to authenticate user"));
		}
		
		await _signInManager.SignInAsync(user, true, "Google");

		var authClaims = await _userManager.GetClaimsAsync(user);

		return _jwtTokenGenerator.CreateToken(authClaims);
	}
}