using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Domain.Events;

public record MessageSent(Message Message) : IDomainEvent;