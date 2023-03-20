using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RoleConfiguration.DataPersistence;
using RoleConfiguration.Repositories;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;

namespace RoleConfiguration.Commands;

public sealed record MemberFileUpdate(string Source, string Path, string Content) : CommandRequest;

public sealed class MemberFileUpdateHandler : IRequestHandler<MemberFileUpdate>
{
    private readonly Deserializer deserializer;
    private readonly ConfigDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IMemberRepository memberRepository;

    public MemberFileUpdateHandler(Deserializer deserializer, ConfigDbContext dbContext, IMapper mapper, IMemberRepository memberRepository)
    {
        this.deserializer = deserializer;
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.memberRepository = memberRepository;
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

                var fileContent = deserializer.DeserializeMember(request.Content, true);

                var members = dbContext.Members!.Where(e => fileContent.Members.Select(m => m.UniqueName).Contains(e.UniqueName)).ToDictionary(e => e.UniqueName);
                foreach (var memberContent in fileContent.Members)
                {
                    Member? member;
                    if (members.TryGetValue(memberContent.UniqueName ?? "", out member!))
                    {
                        try
                        {
                            await memberRepository.Update(member, memberContent, cancellationToken);
                        }
                        catch (HttpRequestException ex)
                        {
                            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                                await memberRepository.Add(member!, memberContent, cancellationToken);
                            else
                                throw;
                        }
                    }
                    else
                    {
                        MemberContent? currentContent;
                        try
                        {
                            (member, currentContent) = await memberRepository.Get(memberContent.UniqueName!, cancellationToken);
                        }
                        catch (HttpRequestException ex)
                        {
                            if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                                throw;
                        }

                        if (member == null)
                        {
                            member = mapper.Map<MemberContent, Member>(memberContent);
                            await memberRepository.Add(member!, memberContent, cancellationToken);
                        }
                        else
                            await dbContext.Members!.AddAsync(member, cancellationToken);
                    }
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