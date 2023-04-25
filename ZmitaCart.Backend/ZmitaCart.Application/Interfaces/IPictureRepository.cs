using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces;

public interface IPictureRepository
{
    public Task AddAsync(int userId, int offerId, IEnumerable<IFormFile> images);
    public Task RemoveAsync(int userId, int offerId, IEnumerable<int> imagesIds);
    public Task RemoveAllAsync(int userId, int offerId);
}