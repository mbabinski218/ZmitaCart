using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record UserJoined(int UserId, string ConnectionId) : IDomainEvent;