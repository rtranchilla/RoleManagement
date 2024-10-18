using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleManager.Queries;

public abstract record AggregateRootCommon<TResult> : IRequest<TResult>;

public sealed class AggregateRootCommonExceptionHandler<TResult>(ILogger logger) : IRequestExceptionHandler<AggregateRootCommon<TResult>, TResult, Exception>
{
    private readonly ILogger logger = logger;

    public async Task Handle(
        AggregateRootCommon<TResult> request, 
        Exception exception, 
        RequestExceptionHandlerState<TResult> state, 
        CancellationToken cancellationToken)
    {
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
    }
}