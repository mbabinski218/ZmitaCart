namespace ZmitaCart.Domain.Common;

public static class GrantType
{
	public const string password = "Password";
	public const string refreshToken = "RefreshToken";
	public const string google = "Google";
	
	public static IEnumerable<string> SupportedGrantTypes
	{
		get
		{
			yield return password;
			yield return refreshToken;
			yield return google;
		}
	}
}