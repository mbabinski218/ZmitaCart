using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Domain.Events;

public record OfferCreated(Offer Offer, IEnumerable<FileStream>? FileStreams) : IDomainEvent;