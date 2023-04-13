namespace ZmitaCart.Infrastructure.Exceptions;

public class InvalidLoginDataException : Exception
{
	public InvalidLoginDataException()
	{

	}
       
	public InvalidLoginDataException(string msg) : base(msg)
	{

	}
}