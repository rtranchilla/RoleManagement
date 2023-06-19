using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleManager.Queries;

public abstract record QueryRequest<TResult> : IRequest<TResult>;

public sealed class QueryExceptionHandler<TResult> : AsyncRequestExceptionHandler<QueryRequest<TResult>, TResult>
{
    private readonly ILogger logger;

    public QueryExceptionHandler(ILogger logger) => this.logger = logger;

    protected override async Task Handle(
        QueryRequest<TResult> request,
        Exception exception,
        RequestExceptionHandlerState<TResult> state,
        CancellationToken cancellationToken) => 
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
}