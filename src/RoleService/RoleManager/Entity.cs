namespace RoleManagement.RoleManagementService;

public interface IEntity
{
    Guid Id { get; }
}

public abstract class Entity<T> : IEquatable<Entity<T>>, IEntity
    where T : Entity<T>
{
    protected Entity(Func<T, Guid> idFunc) => IdFunc = idFunc;

    internal Func<T, Guid> IdFunc { get; }

    public virtual bool Equals(Entity<T>? other) => other is IEntity oEntity && EqualityComparer<Guid>.Default.Equals(IdFunc.Invoke((T)this), oEntity.Id);
    public override bool Equals(object? obj) => obj is Entity<T> entity && Equals(entity);

    public override int GetHashCode() => HashCode.Combine(EqualityComparer<Guid>.Default.GetHashCode(IdFunc.Invoke((T)this)));
    public static bool operator ==(Entity<T>? x, Entity<T>? y) => EqualityComparer<Entity<T>>.Default.Equals(x, y);
    public static bool operator !=(Entity<T>? x, Entity<T>? y) => !EqualityComparer<Entity<T>>.Default.Equals(x, y);

    Guid IEntity.Id => IdFunc.Invoke((T)this);
}