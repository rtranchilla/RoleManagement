namespace RoleManager;

public abstract class AssociatorEntity<T> : Entity<AssociatorEntity<T>>, IEquatable<AssociatorEntity<T>>
    where T : AssociatorEntity<T>
{
    protected AssociatorEntity(Func<T, Guid> id1Func, Func<T, Guid> id2Func) : base(e => id1Func.Invoke((T)e)) => Id2Func = id2Func;

    internal Func<T, Guid> Id2Func { get; }

    public override bool Equals(Entity<AssociatorEntity<T>>? other) => other is AssociatorEntity<T> entity && Equals(entity);
    public virtual bool Equals(AssociatorEntity<T>? other) => base.Equals(other) &&
        EqualityComparer<Guid>.Default.Equals(Id2Func.Invoke((T)this), other.Id2Func.Invoke((T)other));

    public override bool Equals(object? obj) => obj is AssociatorEntity<T> entity && Equals(entity);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), EqualityComparer<Guid>.Default.GetHashCode(Id2Func.Invoke((T)this)));
    public static bool operator ==(AssociatorEntity<T>? x, AssociatorEntity<T>? y) => EqualityComparer<AssociatorEntity<T>>.Default.Equals(x, y);
    public static bool operator !=(AssociatorEntity<T>? x, AssociatorEntity<T>? y) => !EqualityComparer<AssociatorEntity<T>>.Default.Equals(x, y);
}