using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces;

public interface IPictureRepository
{
	public Task<int> AddAsync(int offerId, ICollection<IFormFile> command);
}