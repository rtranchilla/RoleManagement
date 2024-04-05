using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleManager.Queries;

public abstract record AggregateRootCommon<TResult> : IRequest<TResult>;

public sealed class AggregateRootCommonExceptionHandler<TResult>(ILogger logger) : AsyncRequestExceptionHandler<AggregateRootCommon<TResult>, TResult>
{
    private readonly ILogger logger = logger;

    protected override async Task Handle(
        AggregateRootCommon<TResult> request,
        Exception exception,
        RequestExceptionHandlerState<TResult> state,
        CancellationToken cancellationToken)
    {
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
    }
}