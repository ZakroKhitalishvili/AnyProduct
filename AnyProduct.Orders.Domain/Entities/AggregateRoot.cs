using AnyProduct.Orders.Domain;
using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    [Required]
    public Guid AggregateId { get; protected set; }

    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
}
