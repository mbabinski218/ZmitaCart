using ZmitaCart.Application.Services;

namespace ZmitaCart.API.Services;

public class FakeCurrentUserService : ICurrentUserService
{
	public string? UserId => "1";
}