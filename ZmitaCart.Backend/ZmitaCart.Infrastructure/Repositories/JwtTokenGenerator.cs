using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

	public string CreateToken(int userId, string userName, string lastName, string email, string role)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), 
			SecurityAlgorithms.HmacSha512Signature);

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Sub, userId.ToString()),
			new(JwtRegisteredClaimNames.GivenName, userName),
			new(JwtRegisteredClaimNames.FamilyName, lastName),
			new(JwtRegisteredClaimNames.Email, email),
			new("role", role),
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