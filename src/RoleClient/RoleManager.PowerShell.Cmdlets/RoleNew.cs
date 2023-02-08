using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.New, "Role", DefaultParameterSetName = parameterSetByProperty)]
[OutputType(typeof(Role))]
public sealed class RoleNew : RmCmdlet
{
    const string parameterSetByObject = "ByObject";
    const string parameterSetByProperty = "ByProperty";

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Role? Role { get; set; }

    [Parameter(
        Mandatory = false,
        ParameterSetName = parameterSetByProperty)]
    public Guid? Id { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ParameterSetName = parameterSetByProperty)]
    public string? Name { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 1,
        ParameterSetName = parameterSetByProperty)]
    public bool? Reversible { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 2,
        ParameterSetName = parameterSetByProperty)]
    public Guid TreeId { get; set; }

    [Parameter(
        Mandatory = false,
        Position = 3,
        ParameterSetName = parameterSetByProperty)]
    public Guid[] RequiredNodes { get; set; } = Array.Empty<Guid>();

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetByProperty)
            Role = new Role(Id ?? Guid.NewGuid(), Name!, TreeId!)
            {
                RequiredNodes = RequiredNodes
            };

        SendRequest(new RoleCreate(Role!));
    }
}
