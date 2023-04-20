using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Exceptions;
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

    public async Task<int> Create(string name, int? parentId, string? iconName)
    {
        if (await _dbContext.Categories.AnyAsync(c => c.Name == name))
        {
            throw new ArgumentException("Category with input name already exists");
        }

        var category = new Category { Name = name, IconName = iconName };
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

    public async Task<int> Update(int id, string? name, int? parentId, string? iconName)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id) ??
                       throw new NotFoundException("Item does not exist");

        if (name is not null)
        {
            category.Name = name;
        }

        if (iconName is not null)
        {
            category.IconName = iconName;
        }

        if (parentId is not null)
        {
            var parent = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == parentId);
            if (parent is null)
            {
                throw new NotFoundException("Parent category with input id doesn't exist");
            }

            var currentCategory = (await _dbContext.Categories
                .Include(c => c.Children)
                .ToListAsync()).FirstOrDefault(c => c.Id == id);

            CheckChildrenId(currentCategory!, parentId.Value);

            category.Parent = parent;
            category.ParentId = parentId;
        }
        else
        {
            category.ParentId = null;
            category.Parent = null;
        }

        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return category.Id;
    }

    private void CheckChildrenId(Category category, int parentId)
    {
        if (category.Id == parentId)
        {
            throw new ArgumentException("Category cannot have itself as a child");
        }

        if (category.Children is null) return;
        foreach (var child in category.Children)
        {
            CheckChildrenId(child, parentId);
        }
    }

    public async Task Delete(int id)
    {
        var categoryToRemove = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new InvalidDataException("nie ma ");

        await _dbContext.Categories
            .Where(c => c.ParentId == id)
            .ForEachAsync(c => c.ParentId = null);

        _dbContext.Categories.Remove(categoryToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SuperiorCategoryDto>> GetAllSuperiors()
    {
        return await _dbContext.Categories
            .Where(c => c.ParentId == null)
            .ProjectTo<SuperiorCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesBySuperiorId(int superiorId, int? childrenCount)
    {
        var categories = await _dbContext.Categories
            .Where(c => c.Id == superiorId)
            .Include(c => c.Children)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        childrenCount ??= -1; // -1 means all children

        foreach (var category in categories)
        {
            await FillChildren(category.Children, childrenCount.Value);
        }

        return categories;
    }

    //TODO można spróbwać z includem
    private async Task FillChildren(IEnumerable<CategoryDto?>? children, int childrenCount)
    {
        if (childrenCount == 0 || children is null) return;
        childrenCount--;

        foreach (var child in children)
        {
            if (child is null) continue;

            var temp = await _dbContext.Categories
                .Where(c => c.ParentId == child.Id)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            child.Children = temp.Any() && childrenCount != 0 ? temp : null;

            await FillChildren(child.Children, childrenCount);
        }
    }
}