using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleManager.Commands;

public abstract record AggregateRootCommon : IRequest;

public sealed class AggregateRootCommonExceptionHandler : AsyncRequestExceptionHandler<AggregateRootCommon, Unit>
{
    private readonly ILogger logger;

    public AggregateRootCommonExceptionHandler(ILogger logger) => this.logger = logger;

    protected override async Task Handle(AggregateRootCommon request,
        Exception exception,
        RequestExceptionHandlerState<Unit> state,
        CancellationToken cancellationToken)
    {
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
    }
}