using System.Collections.ObjectModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Exceptions;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public OfferRepository(ApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<int> CreateAsync(CreateOfferDto offerDto)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == offerDto.UserId) ??
		           throw new InvalidDataException("User does not exist");

		var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == offerDto.CategoryId) ??
		               throw new InvalidDataException("Category does not exist");

		var offer = _mapper.Map<Offer>(offerDto);
		offer.User = user;
		offer.Category = category;

		await _dbContext.Offers.AddAsync(offer);
		await _dbContext.SaveChangesAsync();

		return offer.Id;
	}

	public async Task<int> UpdateAsync(UpdateOfferDto offerDto)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerDto.Id)
		            ?? throw new InvalidDataException("Offer does not exist");

		if (offer.UserId != offerDto.UserId) throw new UnauthorizedAccessException("User does not have access to this offer");

		offer.Title = offerDto.Title ?? offer.Title;
		offer.Description = offerDto.Description ?? offer.Description;
		offer.Price = offerDto.Price ?? offer.Price;
		offer.Quantity = offerDto.Quantity ?? offer.Quantity;
		offer.Condition = offerDto.Condition ?? offer.Condition;
		offer.IsAvailable = offerDto.IsAvailable ?? offer.IsAvailable;

		await _dbContext.SaveChangesAsync();
		return offer.Id;
	}

	public async Task DeleteAsync(int userId, int offerId)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId)
		            ?? throw new NotFoundException("Offer does not exist");

		if (offer.UserId != userId) throw new UnauthorizedAccessException("User does not have access to this offer");

		_dbContext.Offers.Remove(offer);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<PaginatedList<OfferInfoDto>> GetOffersByCategoryAsync(int categoryId, int? pageNumber = null, int? pageSize = null)
	{
		return (await GetOffersFromSubCategories(categoryId))
			.ToPaginatedList(pageNumber, pageSize);
	}

	public async Task<OfferDto> GetOfferAsync(int id)
	{
		var offer = await _dbContext.Offers
			.Where(o => o.Id == id)
			.Include(o => o.User)
			.Include(o => o.Pictures)
			.FirstOrDefaultAsync() ?? throw new NotFoundException("Offer does not exist");

		var offerDto = _mapper.Map<OfferDto>(offer);

		if (offerDto.PicturesUrls!.Count == 0)
			offerDto.PicturesUrls = null;

		return offerDto;
	}

	public async Task AddToFavoritesAsync(int userId, int offerId)
	{
		var offer = await _dbContext.Offers
			            .Include(u => u.Favorites)
			            .FirstOrDefaultAsync(o => o.Id == offerId)
		            ?? throw new NotFoundException("Offer does not exist");

		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
		           ?? throw new NotFoundException("User does not exist");

		var offerInFavorites = user.Favorites?.FirstOrDefault(f => f.OfferId == offerId);

		if (offerInFavorites is null)
		{
			await _dbContext.Favorites.AddAsync(new UserOffer
			{
				UserId = user.Id,
				OfferId = offer.Id
			});
		}
		else
		{
			_dbContext.Favorites.Remove(offerInFavorites);
		}

		await _dbContext.SaveChangesAsync();
	}

	public async Task<PaginatedList<OfferInfoDto>> GetFavoritesOffersAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		return await _dbContext.Favorites
			.Where(f => f.UserId == userId)
			.Include(f => f.Offer)
			.ThenInclude(o => o.User)
			.Include(f => f.Offer)
			.ThenInclude(o => o.Pictures)
			.Select(f => f.Offer)
			.AsNoTracking()
			.ProjectTo<OfferInfoDto>(_mapper.ConfigurationProvider)
			.ToPaginatedListAsync(pageNumber, pageSize);
	}

	public async Task BuyAsync(int userId, int offerId, int quantity)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId)
		            ?? throw new NotFoundException("Offer does not exist");

		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
		           ?? throw new NotFoundException("User does not exist");

		if (offer.UserId == userId) throw new InvalidDataException("You cannot buy your own offer");
		if (offer.Quantity == 0) throw new InvalidDataException("Offer is not available");
		if (offer.Quantity < quantity) throw new InvalidDataException("Not enough quantity");

		offer.Quantity -= quantity;
		if (offer.Quantity == 0) offer.IsAvailable = false;

		await _dbContext.Bought.AddAsync(new Bought
		{
			UserId = user.Id,
			User = user,
			OfferId = offer.Id,
			Offer = offer,
			Quantity = quantity,
			BoughtAt = DateTimeOffset.Now,
			TotalPrice = offer.Price * quantity
		});

		await _dbContext.SaveChangesAsync();
	}

	public async Task<PaginatedList<BoughtOfferDto>> GetBoughtAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		return await _dbContext.Bought
			.Where(b => b.UserId == userId)
			.Include(b => b.Offer)
			.ProjectTo<BoughtOfferDto>(_mapper.ConfigurationProvider)
			.ToPaginatedListAsync(pageNumber, pageSize);

		// return await _dbContext.Users
		// 	.Where(u => u.Id == userId)
		// 	.Include(u => u.Bought ?? new Collection<Bought>())
		// 	.ThenInclude(b => b.Offer)
		// 	.Select(u => u.Bought)
		// 	.ProjectTo<BoughtOfferDto>(_mapper.ConfigurationProvider)
		// 	.ToPaginatedListAsync(pageNumber, pageSize);
	}

	private async Task<List<OfferInfoDto>> GetOffersFromSubCategories(int categoryId)
	{
		var offers = new List<OfferInfoDto>();
		var category = await _dbContext.Categories
			               .Include(c => c.Children)
			               .FirstOrDefaultAsync(c => c.Id == categoryId)
		               ?? throw new NotFoundException("Category does not exist");

		offers.AddRange(await _dbContext.Offers
			.Where(o => o.CategoryId == categoryId && o.IsAvailable)
			.Include(o => o.User)
			.Include(o => o.Pictures)
			.AsNoTracking()
			.ProjectTo<OfferInfoDto>(_mapper.ConfigurationProvider)
			.ToListAsync());

		if (category.Children is null) return offers;

		foreach (var child in category.Children)
		{
			offers.AddRange(await GetOffersFromSubCategories(child.Id));
		}

		return offers;
	}
}