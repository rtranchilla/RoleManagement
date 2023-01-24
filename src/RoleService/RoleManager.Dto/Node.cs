namespace RoleManager.Dto;

public class Node : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool BaseNode { get; set; }
    public Guid TreeId { get; set; }
}
