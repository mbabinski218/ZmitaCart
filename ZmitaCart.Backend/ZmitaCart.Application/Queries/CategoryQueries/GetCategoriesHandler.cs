using AutoMapper;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.CategoryQueries;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesBySuperiorIdQuery, IEnumerable<CategoryDto>>,
    IRequestHandler<GetCategoriesWithChildrenBySuperiorIdQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesBySuperiorIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetCategoriesBySuperiorId(request.SuperiorId, null);
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesWithChildrenBySuperiorIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetCategoriesBySuperiorId(request.SuperiorId, request.ChildrenCount);
    }
}