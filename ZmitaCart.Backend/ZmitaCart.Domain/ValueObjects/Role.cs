using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.ValueObjects;

public class Role : ValueObject
{
	public static Role Administrator => new("Administrator");
	public static Role User => new("User");

	public string Code { get; private set; } = User.Code;

	private Role(string roleCode)
	{
		Code = roleCode;
	}

	private Role()
	{
		
	}
	
	public static IEnumerable<Role> SupportedRoles
	{
		get
		{
			yield return Administrator;
			yield return User;
		}
	}
	
	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Code;
	}
}