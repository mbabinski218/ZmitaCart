using MediatR;

namespace ZmitaCart.Application.Events;

public record JoinedChat(int Chat) : INotification;