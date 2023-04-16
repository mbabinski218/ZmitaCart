namespace ZmitaCart.Infrastructure.Exceptions;

public class InvalidLoginDataException : Exception
{
	public IEnumerable<string> Errors { get; set; }
	
	public InvalidLoginDataException(IEnumerable<string> errors) : base("Invalid login data")
	{
		Errors = errors;
	}
}