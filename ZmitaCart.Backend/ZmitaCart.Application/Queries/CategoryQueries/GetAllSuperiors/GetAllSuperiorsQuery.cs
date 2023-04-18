using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetAllSuperiors;

public record GetAllSuperiorsQuery() : IRequest<IEnumerable<SuperiorCategoryDto>>;