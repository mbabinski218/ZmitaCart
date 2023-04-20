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
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == offerDto.UserId);
		if (user is null)
		{
			throw new InvalidDataException("User does not exist");
		}

		var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == offerDto.CategoryId);
		if (category is null)
		{
			throw new InvalidDataException("Category does not exist");
		}

		var offer = _mapper.Map<Offer>(offerDto);
		offer.User = user;
		offer.Category = category;

		await _dbContext.Offers.AddAsync(offer);
		await _dbContext.SaveChangesAsync();

		return offer.Id;
	}
}