namespace ZmitaCart.Application.Services;

public interface ICurrentUserService
{
	public string? UserId { get; }
	public string? UserEmail { get; }
	public string? UserFirstName { get; }
	public string? UserLastName { get; }
	public string? UserRole { get; }
}