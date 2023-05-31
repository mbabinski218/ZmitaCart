using FluentResults;
using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces;

public interface IPictureRepository
{
    public Task<Result> AddAsync(int userId, int offerId, IEnumerable<FileStream> filesStreams);
    public Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<int>? imagesIds = null);
}