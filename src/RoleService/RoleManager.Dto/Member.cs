namespace RoleManager.Dto;
public sealed class Member : IEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }

    [Required]
    public string UniqueName { get; set; }
}