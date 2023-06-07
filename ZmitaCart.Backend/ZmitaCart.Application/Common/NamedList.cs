namespace ZmitaCart.Application.Common;

public class NamedList<T>
{
	public string Name { get; set; } = null!;
	public List<T> List { get; set; } = null!;
}