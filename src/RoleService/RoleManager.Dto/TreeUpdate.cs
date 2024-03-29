﻿namespace RoleManager.Dto;

public sealed class TreeUpdate : IEntity
{
    [Required]
    public Guid Id { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
