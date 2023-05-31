using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PictureRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddAsync(int userId, int offerId, IEnumerable<FileStream> filesStreams)
    {
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }

        if (offer.UserId != userId)
        {
            return Result.Fail(new UnauthorizedError("You are not authorized to update an offer."));
        }

        foreach (var fileStream in filesStreams)
        {
            if (fileStream.Length <= 0) continue;

            var creationTime = DateTimeOffset.Now;
            var imageName = $"{offerId}_{creationTime:ddMMyyyyhhmmssfff}_{new Random().Next(0, 10000000)}.{fileStream.Name.Split('.').Last()}";
            var filePath = Path.Combine(Path.GetFullPath("wwwroot"), imageName);
            
            using (var image = await Image.LoadAsync(fileStream))
            {
                if(image.Size.Width > 2000 || image.Size.Height > 2000)
                    image.Mutate(op => op.Resize(new ResizeOptions
                    {
                        Size = new Size(2000, 2000),
                        Mode = ResizeMode.Max,
                        Sampler = LanczosResampler.Lanczos3
                    }));

                await image.SaveAsync(filePath);
            }

            var picture = new Picture
            {
                OfferId = offerId,
                Offer = offer,
                Name = imageName,
                CreationTime = creationTime
            };

            await _dbContext.Pictures.AddAsync(picture);
        }

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<int>? imagesIds = null)
    {
        var offer = await _dbContext.Offers
            .Include(o => o.Pictures)
            .FirstOrDefaultAsync(o => o.Id == offerId);
        
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }

        if (offer.UserId != userId)
        {
            return Result.Fail(new UnauthorizedError("You are not authorized to remove an offer."));
        }

        if (!offer.Pictures.Any())
        {
            return Result.Ok();
        }

        var pictures = offer.Pictures.Where(picture => imagesIds?.Contains(picture.Id) ?? true).ToList();

        if (!pictures.Any())
        {
            return Result.Ok();
        }

        foreach (var filePath in pictures.Select(picture => Path.Combine(Path.GetFullPath("wwwroot"), picture.Name)).Where(File.Exists))
        {
            File.Delete(filePath);
        }

        _dbContext.Pictures.RemoveRange(pictures);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}