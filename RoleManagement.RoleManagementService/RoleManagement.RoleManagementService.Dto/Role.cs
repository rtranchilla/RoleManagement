﻿using System.ComponentModel.DataAnnotations;

namespace RoleManagement.RoleManagementService.Dto;

public class Role : IEntity
{
    public Guid Id { get; set; }
    public bool Reversible { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid TreeId { get; set; }
}