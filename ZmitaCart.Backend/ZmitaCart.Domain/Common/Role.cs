namespace ZmitaCart.Domain.Common;

public class Role
{
	public static Role Administrator => new("Administrator");
	public static Role User => new("User");

	public string Code { get; private set; }

	private Role(string roleCode)
	{
		Code = roleCode;
	}

	public static IEnumerable<Role> SupportedRoles
	{
		get
		{
			yield return Administrator;
			yield return User;
		}
	}
}