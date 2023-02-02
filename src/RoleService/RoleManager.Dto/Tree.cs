namespace RoleManager.Dto;

public sealed class Tree : IEntity
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
