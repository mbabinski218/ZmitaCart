using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Interfaces;

public interface IOfferRepository
{
	public Task<int> CreateAsync(CreateOfferDto command);
}
