using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Dtos.User;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Domain.ValueObjects;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Exceptions;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IMapper _mapper;
	private readonly PasswordManager _passwordManager;

	public UserRepository(ApplicationDbContext dbContext, IJwtTokenGenerator jwtTokenGenerator, 
		IMapper mapper, IOptions<PasswordManager> passwordManager)
	{
		_dbContext = dbContext;
		_jwtTokenGenerator = jwtTokenGenerator;
		_mapper = mapper;
		_passwordManager = passwordManager.Value;
	}

	public async Task RegisterAsync(RegisterUserDto userDto)
	{
		if (await _dbContext.Users.AnyAsync(user => user.Email == userDto.Email))
			throw new InvalidDataException("Email already used");

		_passwordManager.CreatePasswordHash(userDto.Password, out var passwordHash, out var passwordSalt);
		
		var user = new User
		{
			Email = userDto.Email,
			FirstName = userDto.FirstName,
			LastName = userDto.LastName,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			Role = Role.User,
		};
        
		await _dbContext.Users.AddAsync(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<string> LoginAsync(LoginUserDto userDto)
	{
		var (passwordHash, passwordSalt) = await GetPasswordHashAndSalt(userDto.Email);

		if (!_passwordManager.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
			throw new InvalidLoginDataException("Wrong email or password");

		var userClaims = await GetUserClaims(userDto.Email);

		return _jwtTokenGenerator.CreateToken(userClaims);
	}

	public async Task ChangeRoleAsync(string userEmail, Role newRole)
	{
		var user = await _dbContext.Users
			.Where(user => user.Email == userEmail)
			.FirstOrDefaultAsync();

		if (user == null)
			throw new InvalidLoginDataException("Wrong email");

		user.Role = newRole;
		await _dbContext.SaveChangesAsync();
	}

	private async Task<(byte[], byte[])> GetPasswordHashAndSalt(string userEmail)
	{
		var data = await _dbContext.Users
			.Where(user => user.Email == userEmail)
			.Select(user => new { user.PasswordHash, user.PasswordSalt })
			.FirstOrDefaultAsync();

		if (data == null)
			throw new InvalidLoginDataException("Wrong email or password");
		
		return (data.PasswordHash, data.PasswordSalt);
	}
	
	private async Task<UserClaimsDto> GetUserClaims(string userEmail)
	{
		var userClaims = await _dbContext.Users
			.Where(user => user.Email == userEmail)
			.ProjectTo<UserClaimsDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();

		if (userClaims == null)
			throw new DatabaseException("Transaction error");

		return userClaims;
	}
}