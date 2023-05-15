using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Interfaces;

public interface IUserRepository
{
	public Task RegisterAsync(RegisterUserDto registerUserDto);
	public Task<string> LoginAsync(LoginUserDto loginUserDto);
	public Task LogoutAsync();
	public Task AddRoleAsync(string userEmail, string role);
	public Task<string> ExternalAuthenticationAsync(ExternalAuthDto externalAuthDto);
	public Task UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address);
	public Task<int> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment);
	public Task<int> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment);
	public Task DeleteFeedbackAsync(int feedbackId);
	public Task<PaginatedList<FeedbackDto>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize);
}