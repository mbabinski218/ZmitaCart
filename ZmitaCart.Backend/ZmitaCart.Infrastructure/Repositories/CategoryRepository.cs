using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence;

namespace ZmitaCart.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<int> Create(string name, int? parentId)
    {
        if (await _dbContext.Categories.AnyAsync(c => c.Name == name))
        {
            throw new ArgumentException("Category with input name already exists");
        }

        var category = new Category { Name = name };
        if (parentId is null)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category.Id;
        }

        var parent = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == parentId);
        if (parent is null)
        {
            throw new ArgumentException("Parent category with input id doesn't exist");
        }

        category.Parent = parent;
        category.ParentId = parentId;
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return category.Id;
    }

    public Task<int> Update(int id, string? name, int? parentId)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SuperiorCategoryDto>> GetAllSuperiorsNames()
    {
        return await _dbContext.Categories
            .Where(c => c.ParentId == null)
            .ProjectTo<SuperiorCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesBySuperiorId(int superiorId, int? childrenCount)
    {
        var categories = _dbContext.Categories
            .Where(c => c.Id == superiorId)
            .Include(c => c.Children)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider);

        if (childrenCount is null)
        {
            return await categories.ToListAsync();
        }

        throw new Exception("fdasf");
        //GetCategoriesBySuperiorId(superiorId, childrenCount);
    }
}