using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.UserCommands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public RegisterUserHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var registerUser = _mapper.Map<RegisterUserDto>(request);

		return await _userRepository.RegisterAsync(registerUser);
	}
}