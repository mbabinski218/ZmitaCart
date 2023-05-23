using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.UserQueries.LoginUser;

public record LoginUserQuery : IRequest<Result<string>>
{
	public string Email { get; init; } = null!;
	public string Password { get; init; } = null!;
}