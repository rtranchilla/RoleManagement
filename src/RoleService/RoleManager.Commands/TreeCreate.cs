﻿using AutoMapper;
using MediatR;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands;

public sealed record TreeCreate(Dto.Tree Tree) : AggregateRootCreate;
public sealed class TreeCreateHandler : AggregateRootCreateHandler<TreeCreate, Tree, Dto.Tree>
{
    private readonly IPublisher publisher;

    public TreeCreateHandler(RoleDbContext dbContext, IMapper mapper, IPublisher publisher) : base(dbContext, mapper) => this.publisher = publisher;

    protected override Dto.Tree GetDto(TreeCreate request) => request.Tree;
    protected override Task PostSave(Tree aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => 
        publisher.Publish(new TreeCreated(aggregateRoot), cancellationToken);
}