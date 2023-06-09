using MediatR;

namespace ZmitaCart.Domain.Events;

public record JoinedChat(int Chat, string UserId) : INotification;