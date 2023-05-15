using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Commands.UserCommands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public RegisterUserHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var registerUser = _mapper.Map<RegisterUserDto>(request);

		await _userRepository.RegisterAsync(registerUser);
	}
}