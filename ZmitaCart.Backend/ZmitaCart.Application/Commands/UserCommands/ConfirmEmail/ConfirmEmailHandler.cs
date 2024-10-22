using System.Web;
using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.UserCommands.ConfirmEmail;

public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
	private readonly IUserRepository _userRepository;

	public ConfirmEmailHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.FindByIdAsync(request.Id);

		if (user.IsFailed)
		{
			return user.ToResult();
		}
		
		var decodedToken = HttpUtility.UrlDecode(request.Token);
		
		return await _userRepository.ConfirmEmailAsync(user.Value, decodedToken);
	}
}