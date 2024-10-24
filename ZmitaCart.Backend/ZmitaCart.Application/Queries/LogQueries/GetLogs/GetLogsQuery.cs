using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;

namespace ZmitaCart.Application.Queries.LogQueries.GetLogs;

public record GetLogsQuery (
		int? PageNumber, 
		int? PageSize
	) : IRequest<Result<PaginatedList<LogDto>>>;