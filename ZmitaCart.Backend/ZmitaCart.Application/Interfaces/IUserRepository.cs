using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public Task<Result> RegisterAsync(RegisterUserDto dto);
	public Task<Result<TokensDto>> LoginAsync(LoginUserDto loginUserDto);
	public Task<Result<TokensDto>> LoginWithRefreshTokenAsync(string? userId, string refreshToken);
	public Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken);
	public Task<Result> LogoutAsync();
	public Task<Result> AddRoleAsync(string userEmail, string role);
	public Task<Result> UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address);
	public Task<Result<int>> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment);
	public Task<Result<int>> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment);
	public Task<Result> DeleteFeedbackAsync(int feedbackId);
	public Task<Result<PaginatedList<FeedbackDto>>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize);
}