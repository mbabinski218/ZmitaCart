﻿using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;
using ZmitaCart.Application.Interfaces.Services;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.DbContexts;

namespace ZmitaCart.Infrastructure.Services;

public sealed class UserEventLoggerService : IUserEventLoggerService
{
	private readonly ILogger<UserEventLoggerService> _logger;
	private readonly LogDbContext _logDbContext;
	private readonly ICurrentUserService _currentUserService;

	public UserEventLoggerService(ILogger<UserEventLoggerService> logger, LogDbContext logDbContext, ICurrentUserService currentUserService)
	{
		_logger = logger;
		_logDbContext = logDbContext;
		_currentUserService = currentUserService;
	}
	
	public async Task LogUserLoggedInSuccessAsync(string details, int userId, string userEmail)
	{
		var log = UserLog.CreateLoginSuccess(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userId, userEmail);
		await AddLogAsync(log);
	}
	
	public async Task LogUserLoggedInFailureAsync(string details, string? userEmail)
	{
		var log = UserLog.CreateLoginFailed(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userEmail);
		await AddLogAsync(log);
	}

	public async Task LogUserLoggedOutSuccessAsync(string details, int userId, string userEmail)
	{
		var log = UserLog.CreateLogoutSuccess(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userId, userEmail);
		await AddLogAsync(log);
	}
	
	public Task LogUserLoggedOutFailureAsync(string details, int userId)
	{
		var log = UserLog.CreateLogoutFailed(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userId);
		return AddLogAsync(log);
	}
	
	public async Task LogUserRegisteredSuccessAsync(string details, int userId, string userEmail)
	{
		var log = UserLog.CreateRegisterSuccess(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userId, userEmail);
		await AddLogAsync(log);
	}
	
	public async Task LogUserRegisteredFailureAsync(string details, string userEmail)
	{
		var log = UserLog.CreateRegisterFailed(details, _currentUserService.IpAddress ?? "unknown", _currentUserService.UserAgent ?? "unknown", userEmail);
		await AddLogAsync(log);
	}
	
	private async Task AddLogAsync(UserLog userLog)
	{
		_logDbContext.UserLogs.Add(userLog);
		if (await _logDbContext.SaveChangesAsync() <= 0)
		{
			_logger.LogCritical("Failed to save log");
		}
	}
	
	public async Task<Result<PaginatedList<LogDto>>> GetLogsAsync(int? pageNumber, int? pageSize)
	{
		var logs = await _logDbContext.UserLogs
			.OrderByDescending(x => x.Timestamp)
			.ProjectToType<LogDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
		
		return logs;
	}
}