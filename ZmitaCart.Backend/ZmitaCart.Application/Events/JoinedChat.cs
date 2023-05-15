using MediatR;

namespace ZmitaCart.Application.Events;

public record JoinedChat(string Chat) : INotification;