using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record MessageSent(string UserId, int ChatId, string Text) : IDomainEvent;