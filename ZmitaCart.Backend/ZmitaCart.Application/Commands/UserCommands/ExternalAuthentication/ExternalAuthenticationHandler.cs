using AutoMapper;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Commands.UserCommands.ExternalAuthentication;

public class ExternalAuthenticationHandler : IRequestHandler<ExternalAuthenticationCommand, string>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public ExternalAuthenticationHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<string> Handle(ExternalAuthenticationCommand request, CancellationToken cancellationToken)
	{
		var externalAuthDto = _mapper.Map<ExternalAuthenticationCommand, ExternalAuthDto>(request);
		return await _userRepository.ExternalAuthenticationAsync(externalAuthDto);
	}
}