using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleManager.Commands;

public abstract record AggregateRootCommon : IRequest;

public sealed class AggregateRootCommonExceptionHandler(ILogger<AggregateRootCommon> logger) : IRequestExceptionHandler<AggregateRootCommon, Unit, Exception>
{
    public async Task Handle(AggregateRootCommon request, Exception exception, RequestExceptionHandlerState<Unit> state, CancellationToken cancellationToken) => 
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
}