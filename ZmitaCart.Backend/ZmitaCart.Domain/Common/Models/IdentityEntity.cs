using Microsoft.AspNetCore.Identity;

namespace ZmitaCart.Domain.Common.Models;

public class IdentityEntity<TId> : IdentityUser<TId>, IHasDomainEvent
	where TId : IEquatable<TId>
{
	private readonly List<IDomainEvent> _domainEvents = new();
	public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
	
	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	public void AddDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}