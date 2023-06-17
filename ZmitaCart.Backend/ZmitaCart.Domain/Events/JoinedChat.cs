using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record JoinedChat(int UserId, int ChatId) : IDomainEvent;