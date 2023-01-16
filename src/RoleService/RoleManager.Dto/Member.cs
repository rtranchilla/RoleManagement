using System.ComponentModel.DataAnnotations;

namespace RoleManagement.RoleManagementService.Dto;
public class Member : IEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }

    [Required]
    public string UniqueName { get; set; }
}