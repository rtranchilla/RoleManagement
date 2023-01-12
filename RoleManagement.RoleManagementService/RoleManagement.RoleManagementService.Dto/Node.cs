using System.ComponentModel.DataAnnotations;

namespace RoleManagement.RoleManagementService.Dto;

public class Node : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool BaseNode { get; set; }
    [Required]
    public Guid TreeId { get; set; }
}
