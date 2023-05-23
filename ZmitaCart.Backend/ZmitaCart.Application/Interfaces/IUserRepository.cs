using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public Task<Result> RegisterAsync(RegisterUserDto registerUserDto);
	public Task<Result<string>> LoginAsync(LoginUserDto loginUserDto);
	public Task<Result> LogoutAsync();
	public Task<Result> AddRoleAsync(string userEmail, string role);
	public Task<Result<string>> ExternalAuthenticationAsync(ExternalAuthDto externalAuthDto);
	public Task<Result> UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address);
	public Task<Result<int>> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment);
	public Task<Result<int>> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment);
	public Task<Result> DeleteFeedbackAsync(int feedbackId);
	public Task<Result<PaginatedList<FeedbackDto>>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize);
}