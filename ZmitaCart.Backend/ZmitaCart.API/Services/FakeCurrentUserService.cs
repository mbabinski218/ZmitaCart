using ZmitaCart.Application.Services;

namespace ZmitaCart.API.Services;

public class FakeCurrentUserService : ICurrentUserService
{
	public string? UserId => "1";
	public string? UserEmail => "fake@email.com";
	public string? UserFirstName => "FakeFirstName";
	public string? UserLastName => "FakeLastName";
	public string? UserRole => "fake";
	public string? Expires => "123456";
}