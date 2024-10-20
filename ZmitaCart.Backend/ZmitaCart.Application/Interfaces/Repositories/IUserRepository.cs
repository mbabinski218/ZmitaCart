using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces.Repositories;

public interface IUserRepository
{
	Task<Result<UserDataDto>> GetDataAsync(int id);
	Task<Result> RegisterAsync(RegisterUserDto dto);
	Task<Result<TokensDto>> LoginAsync(LoginUserDto loginUserDto);
	Task<Result<TokensDto>> LoginWithRefreshTokenAsync(string refreshToken);
	Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken);
	Task<Result> LogoutAsync(int userId);
	Task<Result> AddRoleAsync(string userEmail, string role);
	Task<Result> UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address);
	Task<Result<int>> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment);
	Task<Result<int>> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment);
	Task<Result> DeleteFeedbackAsync(int feedbackId);
	Task<Result<PaginatedList<FeedbackDto>>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize);
}