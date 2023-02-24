using AutoMapper;
using Dapr.Client;
using Google.Type;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Storage;
using RoleConfiguration.DataPersistence;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;

namespace RoleConfiguration.Commands;

public sealed record MemberFileUpdate(string Source, string Path, string Content) : IRequest;

public sealed class MemberFileUpdateHandler : IRequestHandler<MemberFileUpdate>
{
    private readonly Deserializer deserializer;
    private readonly ConfigDbContext dbContext;
    private readonly IMapper mapper;
    private readonly DaprClient daprClient;

    public MemberFileUpdateHandler(Deserializer deserializer, ConfigDbContext dbContext, IMapper mapper, DaprClient daprClient)
    {
        this.deserializer = deserializer;
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.daprClient = daprClient;
    }

    public async Task<Unit> Handle(MemberFileUpdate request, CancellationToken cancellationToken)
    {

        using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
            try
            {
                var fileRecord = dbContext.Files!.FirstOrDefault(e => e.Path == request.Path && e.Source!.Name == request.Source);
                if (fileRecord == null)
                {
                    var source = dbContext.Sources!.First(e => e.Name == request.Source);
                    fileRecord = new File(request.Path, source.Id);
                    await dbContext.Files!.AddAsync(fileRecord, cancellationToken);
                }

                var fileContent = deserializer.DeserializeMember(request.Content);

                var members = dbContext.Members!.Where(e => fileContent.Members.Select(m => m.UniqueName).Contains(e.UniqueName)).ToDictionary(e => e.UniqueName);
                foreach (var memberContent in fileContent.Members)
                {
                    RoleManager.Dto.Member memberDto;
                    Member member;
                    if (members.TryGetValue(memberContent.UniqueName ?? "", out member!))
                    {
                        memberDto = mapper.Map<Member, RoleManager.Dto.Member>(member);
                        memberDto = mapper.Map(memberContent, memberDto);
                    }
                    else
                    {
                        memberDto = await daprClient.InvokeMethodAsync<string, RoleManager.Dto.Member>(HttpMethod.Get, "RoleManager", "Member/ByUniqueName", memberContent.UniqueName ?? "", cancellationToken);
                        memberDto = mapper.Map(memberContent, memberDto);
                        if (memberDto.Id == default)
                            memberDto.Id = Guid.NewGuid();
                        member = mapper.Map<RoleManager.Dto.Member, Member>(memberDto);
                        await dbContext.Members!.AddAsync(member, cancellationToken);
                    }

                    await daprClient.InvokeMethodAsync(HttpMethod.Post, "RoleManager", "Member", memberDto, cancellationToken);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }


                
        return Unit.Value;
    }
}