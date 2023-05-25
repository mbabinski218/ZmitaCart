using ZmitaCart.Domain.Common;

namespace ZmitaCart.Application.Events;

public record MessageSent(string UserId, int ConversationId, string Text) : IDomainEvent;