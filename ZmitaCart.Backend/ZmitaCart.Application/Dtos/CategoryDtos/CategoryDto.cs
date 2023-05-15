namespace ZmitaCart.Application.Dtos.CategoryDtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<CategoryDto?>? Children { get; set; }
}