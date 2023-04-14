using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.ValueObjects;

public class Address : ValueObject
{
	public string? Country { get; protected set; }
	public string? City { get; protected set; }
	public string? Street { get; protected set; }
	public int? PostalCode { get; protected set; }
	public int? HouseNumber { get; protected set; }
	public int? ApartmentNumber { get; protected set; }

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Country;
		yield return City;
		yield return Street;
		yield return PostalCode;
		yield return HouseNumber;
		yield return ApartmentNumber;
	}
}