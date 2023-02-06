namespace RoleManager.Dto;
public sealed class MemberUpdate : IEntity
{
    [Required]
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
}