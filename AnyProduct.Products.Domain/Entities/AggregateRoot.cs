
namespace AnyProduct.Products.Domain.Entities;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();
    public Guid AggregateId { get; protected set; }

    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
}
