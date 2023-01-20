namespace RoleManager;

public abstract class EntityWithId : Entity<EntityWithId>, IEquatable<EntityWithId>, IEntity
{
    protected EntityWithId() : this(Guid.NewGuid()) { }
    protected EntityWithId(Guid id) : base(e => e.Id) => Id = id;
    public Guid Id { get; private set; }
    Guid IEntity.Id => Id;

    public bool Equals(EntityWithId? other) => EqualityComparer<Guid>.Default.Equals(Id, other?.Id ?? Guid.Empty);
    public override bool Equals(object? obj) => obj is EntityWithId entity && Equals(entity);
    public override int GetHashCode() => 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
    public static bool operator ==(EntityWithId? x, EntityWithId? y) => EqualityComparer<EntityWithId>.Default.Equals(x, y);
    public static bool operator !=(EntityWithId? x, EntityWithId? y) => !EqualityComparer<EntityWithId>.Default.Equals(x, y);
}