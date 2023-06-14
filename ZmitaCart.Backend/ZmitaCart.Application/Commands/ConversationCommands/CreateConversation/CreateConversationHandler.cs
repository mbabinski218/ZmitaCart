using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Application.Services;

namespace ZmitaCart.Application.Commands.ConversationCommands.CreateConversation;

public class CreateConversationHandler : IRequestHandler<CreateConversationCommand, Result<int>>
{
	private readonly IConversationRepository _conversationRepository;
	private readonly ICurrentUserService _currentUserService;

	public CreateConversationHandler(IConversationRepository conversationRepository, ICurrentUserService currentUserService)
	{
		_conversationRepository = conversationRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<int>> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail("Please log in to create a conversation.");
		}

		var userId = int.Parse(_currentUserService.UserId);
		
		var chat = await _conversationRepository.IsChatExistsAsync(request.OfferId, userId);
		
		return chat != 0 ? chat : await _conversationRepository.CreateConversationAsync(request.OfferId, userId);
	}
}