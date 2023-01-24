﻿namespace RoleManager.Dto;

public class Tree : IEntity
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
