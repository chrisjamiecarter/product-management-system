namespace ProductManagement.Domain.Primitives;

/// <summary>
/// Represents the base class for all entities.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }

    public static bool operator ==(Entity? a, Entity? b)
    {
        return a is not null && b is not null && a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b)
    {
        return !(a == b);
    }

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
