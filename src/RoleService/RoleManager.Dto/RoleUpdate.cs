﻿namespace RoleManager.Dto;

public sealed class RoleUpdate : IEntity
{
    public Guid Id { get; set; }
    public bool Reversible { get; set; }
    public Guid[] RequiredNodes { get; set; }
}
