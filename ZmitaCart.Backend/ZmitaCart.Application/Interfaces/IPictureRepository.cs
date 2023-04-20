using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces;

public interface IPictureRepository
{
    public Task AddAsync(int offerId, IEnumerable<IFormFile> images);
}