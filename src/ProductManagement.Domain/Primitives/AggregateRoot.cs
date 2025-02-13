namespace ProductManagement.Domain.Primitives;

/// <summary>
/// Represents the base class for aggregate roots.
/// </summary>
public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) : base(id) { }
}
