using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record JoinedChat(string Chat) : IDomainEvent;