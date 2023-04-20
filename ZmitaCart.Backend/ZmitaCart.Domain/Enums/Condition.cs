using System.ComponentModel;

namespace ZmitaCart.Domain.Enums;

public static class Condition
{
	public const string brandNew = "New";
	public const string used = "Used";
	public const string good = "Good";
	
	public static IEnumerable<string> SupportedConditions
	{
		get
		{
			yield return brandNew;
			yield return used;
			yield return good;
		}
	}
}