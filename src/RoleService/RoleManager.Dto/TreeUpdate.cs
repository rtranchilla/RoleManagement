namespace RoleManager.Dto;

public class TreeUpdate : IEntity
{
    public Guid Id { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
