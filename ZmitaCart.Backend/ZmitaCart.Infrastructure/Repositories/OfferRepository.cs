using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
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
		
		if(offer.UserId != offerDto.UserId) throw new UnauthorizedAccessException("User does not have access to this offer");
		
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
		            ?? throw new InvalidDataException("Offer does not exist");
		
		if(offer.UserId != userId) throw new UnauthorizedAccessException("User does not have access to this offer");
		
		_dbContext.Offers.Remove(offer);
		await _dbContext.SaveChangesAsync();
	}
}