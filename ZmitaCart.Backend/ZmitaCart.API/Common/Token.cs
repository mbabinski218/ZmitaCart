using Microsoft.EntityFrameworkCore.Query;

namespace ZmitaCart.API.Common;

public class TokenToReturn
{
	public string Token { get; init; }

	public TokenToReturn(string value)
	{
		Token = value;
	}
}