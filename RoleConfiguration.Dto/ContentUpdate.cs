using System.ComponentModel.DataAnnotations;

namespace RoleConfiguration.Dto;

public sealed class ContentUpdate
{
    [Required]
    public string Path { get; set; }

    public string Content { get; set; }
}