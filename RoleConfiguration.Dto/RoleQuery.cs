using System.ComponentModel.DataAnnotations;

namespace RoleConfiguration.Dto;

public class RoleQuery
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Tree { get; set; }
}
