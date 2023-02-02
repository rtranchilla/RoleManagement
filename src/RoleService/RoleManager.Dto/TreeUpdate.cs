namespace RoleManager.Dto;

public sealed class TreeUpdate : IEntity
{
    public Guid Id { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
