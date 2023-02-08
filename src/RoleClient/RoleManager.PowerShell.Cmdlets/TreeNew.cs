using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.New, "Tree", DefaultParameterSetName = parameterSetByProperty)]
[OutputType(typeof(Tree))]
public sealed class TreeNew : RmCmdlet
{
    const string parameterSetByObject = "ByObject";
    const string parameterSetByProperty = "ByProperty";

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Tree? Tree { get; set; }

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
        Mandatory = false,
        Position = 1,
        ParameterSetName = parameterSetByProperty)]
    public Guid[] RequiredNodes { get; set; } = Array.Empty<Guid>();

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetByProperty)
            Tree = new Tree(Id ?? Guid.NewGuid(), Name!)
            {
                RequiredNodes = RequiredNodes
            };

        SendRequest(new TreeCreate(Tree!));
        WriteObject(Tree);
    }
}
