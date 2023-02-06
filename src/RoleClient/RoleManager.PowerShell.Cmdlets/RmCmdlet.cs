using MediatR;
using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

public abstract class RmCmdlet : PSCmdlet
{
    [Parameter()]
    public Uri? ConnectionUri { get; set; }

    private object? currentTarget = null;

    protected TResponse SendRequest<TResponse>(IRequest<TResponse> request)
    {
        currentTarget = request;
        var task = SendRequestAsync(request);
        task.Wait();
        currentTarget = null;
        return task.Result;
    }

    protected Task<TResponse> SendRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is RmRequest<TResponse> rmRequest)
            rmRequest.ConnectionUri = ConnectionUri;

        return Services.Mediator.Send(request, cancellationToken);
    }

    protected virtual void BeginProcessingErrorHandling() { }
    protected override void BeginProcessing()
    {
        try
        {
            BeginProcessingErrorHandling();
            base.BeginProcessing();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    protected virtual void ProcessRecordErrorHandling() { }
    protected override void ProcessRecord()
    {
        try
        {
            ProcessRecordErrorHandling();
            base.ProcessRecord();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    protected virtual void EndProcessingErrorHandling() { }
    protected override void EndProcessing()
    {
        try
        {
            EndProcessingErrorHandling();
            base.EndProcessing();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    protected virtual void StopProcessingErrorHandling() { }
    protected override void StopProcessing()
    {
        try
        {
            StopProcessingErrorHandling();
            base.StopProcessing();
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    private void WriteError(Exception ex)
    {
        if (ex is ArgumentException)
            WriteError(new ErrorRecord(ex, ex.Message, ErrorCategory.InvalidArgument, currentTarget));
        else
            WriteError(new ErrorRecord(ex, ex.Message, ErrorCategory.NotSpecified, currentTarget));
    }
}
