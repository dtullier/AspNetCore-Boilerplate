using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApi_Boilerplate.MediatR.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
          : IPipelineBehavior<TRequest, TResponse>
              where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.Log(LogLevel.Information, $"Handling {typeof(TRequest).Name}");
            var response = await next();
            _logger.Log(LogLevel.Information, $"Handled {typeof(TRequest).Name}");
            return response;
        }
    }
}
