using ZmitaCart.Domain.Common;

namespace ZmitaCart.Domain.Events;

public record LeftChat(int UserId) : IDomainEvent;