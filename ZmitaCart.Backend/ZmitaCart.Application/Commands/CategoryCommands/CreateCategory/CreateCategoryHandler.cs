using AutoMapper;
using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Create(request.Name!, request.ParentId);
    }
}