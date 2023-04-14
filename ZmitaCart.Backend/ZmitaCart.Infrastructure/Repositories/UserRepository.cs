using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
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

	public async Task RegisterAsync(RegisterUserDto registerUserDto)
	{
		if (await _dbContext.Users.AnyAsync(user => user.Email == registerUserDto.Email))
			throw new InvalidDataException("Email already used");

		_passwordManager.CreatePasswordHash(registerUserDto.Password, out var passwordHash, out var passwordSalt);
		
		var user = new User
		{
			Email = registerUserDto.Email,
			FirstName = registerUserDto.FirstName,
			LastName = registerUserDto.LastName,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			Role = Role.User,
		};
        
		await _dbContext.Users.AddAsync(user);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<string> LoginAsync(LoginUserDto loginUserDto)
	{
		var (passwordHash, passwordSalt) = await GetPasswordHashAndSalt(loginUserDto.Email);

		if (!_passwordManager.VerifyPasswordHash(loginUserDto.Password, passwordHash, passwordSalt))
			throw new InvalidLoginDataException("Wrong email or password");

		var userClaims = await GetUserClaims(loginUserDto.Email);

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
			throw new DatabaseException("Email not found in database");

		return userClaims;
	}
}