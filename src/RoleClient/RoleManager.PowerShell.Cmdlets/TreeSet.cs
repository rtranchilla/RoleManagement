using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Set, "Tree")]
[OutputType(typeof(Tree))]
public sealed class TreeSet : RmCmdlet
{
    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true)]
    public Tree? Tree { get; set; }

    [Parameter(
        Mandatory = false,
        Position = 1)]
    public Guid[]? RequiredNodes { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (RequiredNodes != null)
            Tree!.RequiredNodes = RequiredNodes;
        SendRequest(new TreeUpdate(Tree!));
        WriteObject(Tree);
    }
}
