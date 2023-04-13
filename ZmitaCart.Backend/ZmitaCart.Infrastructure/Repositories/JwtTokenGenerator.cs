using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZmitaCart.Application.Dtos.User;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Infrastructure.Common;

namespace ZmitaCart.Infrastructure.Repositories;

public class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly JwtSettings _jwtSettings;

	public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
	{
		_jwtSettings = jwtSettings.Value;
	}

	public string CreateToken(UserClaimsDto userClaims)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), 
			SecurityAlgorithms.HmacSha512Signature);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, userClaims.Id.ToString()),
			new(JwtRegisteredClaimNames.GivenName, userClaims.FirstName),
			new(JwtRegisteredClaimNames.Email, userClaims.Email),
			new("role", userClaims.Role),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};
		
		var token = new JwtSecurityToken
		(
			issuer: _jwtSettings.Issuer,
			claims: claims,
			expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireTimeInMinutes),
			signingCredentials: signingCredentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}