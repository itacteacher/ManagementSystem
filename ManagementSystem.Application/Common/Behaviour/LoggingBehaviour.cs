using MediatR;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Application.Common.Behaviour;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour (ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle (TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var requestGuid = Guid.NewGuid().ToString();

        var requestNameWtihGuid = $"[{requestName}] [{requestGuid}]";

        _logger.LogInformation("[Request Start] {requestNameWtihGuid}", requestNameWtihGuid);

        TResponse response;

        try
        {
            response = await next();
        }
        finally
        {
            _logger.LogInformation("[Request End] {requestNameWtihGuid}", requestNameWtihGuid);
        }

        return response;
    }
}
