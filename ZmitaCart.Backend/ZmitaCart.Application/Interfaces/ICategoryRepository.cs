using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Interfaces;

public interface ICategoryRepository
{
    Task<int> Create(string name, int? parentId, string? iconName);
    Task<int> Update(int id, string? name, int? parentId, string? iconName);
    Task Delete(int id);
    Task<IEnumerable<SuperiorCategoryDto>> GetAllSuperiors();
    Task<IEnumerable<CategoryDto>> GetCategoriesBySuperiorId(int superiorId, int? childrenCount);
}