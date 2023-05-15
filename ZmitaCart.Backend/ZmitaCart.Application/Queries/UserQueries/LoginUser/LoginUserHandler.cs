using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces;

namespace ZmitaCart.Application.Queries.UserQueries.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserQuery, string>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public LoginUserHandler(IUserRepository userRepository, IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}
	public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
	{
		var loginUser = _mapper.Map<LoginUserDto>(request);

		return await _userRepository.LoginAsync(loginUser);
	}
}