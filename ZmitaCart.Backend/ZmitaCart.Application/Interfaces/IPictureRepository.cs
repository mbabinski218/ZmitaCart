using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces;

public interface IPictureRepository
{
    public Task AddAsync(int userId, int offerId, IEnumerable<IFormFile> files);
    public Task RemoveAsync(int userId, int offerId, IEnumerable<int>? imagesIds = null);
}