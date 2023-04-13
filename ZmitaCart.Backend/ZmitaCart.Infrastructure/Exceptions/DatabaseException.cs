namespace ZmitaCart.Infrastructure.Exceptions;

public class DatabaseException: Exception
{
	public DatabaseException()
	{

	}
       
	public DatabaseException(string msg) : base(msg)
	{

	}
}