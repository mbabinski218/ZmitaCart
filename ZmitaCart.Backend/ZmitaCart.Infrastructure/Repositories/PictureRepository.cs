using AutoMapper;
using Microsoft.AspNetCore.Http;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public PictureRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(int offerId, IEnumerable<IFormFile> images)
    {
        foreach (var image in images)
        {
            if (image.Length <= 0) return;

            var imageName =
                $"{offerId}_{DateTimeOffset.Now:ddMMyyyyhhmmssfff}_{new Random().Next(0, 10000000)}.{image.FileName.Split('.').Last()}";
            var filePath = Path.Combine(Path.GetFullPath("wwwroot"), imageName);

            await using (var stream = new FileStream(filePath, FileMode.Append))
            {
                await image.CopyToAsync(stream);
            }

            var picture = new Picture
            {
                OfferId = offerId,
                Name = imageName
            };

            await _dbContext.Pictures.AddAsync(picture);
        }

        await _dbContext.SaveChangesAsync();
    }
}