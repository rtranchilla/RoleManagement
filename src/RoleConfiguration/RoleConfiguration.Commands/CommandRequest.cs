using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace RoleConfiguration.Commands;

public abstract record CommandRequest : IRequest;

public sealed class CommandRequestExceptionHandler : AsyncRequestExceptionHandler<CommandRequest, Unit>
{
    private readonly ILogger logger;

    public CommandRequestExceptionHandler(ILogger logger) => this.logger = logger;

    protected override async Task Handle(CommandRequest request,
        Exception exception,
        RequestExceptionHandlerState<Unit> state,
        CancellationToken cancellationToken)
    {
        await Task.Run(() => logger.LogError(exception, exception.Message), cancellationToken);
    }
}