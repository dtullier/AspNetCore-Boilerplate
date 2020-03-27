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

//code for adding data to logs

//var sb = new StringBuilder();
//foreach(var prop in typeof(TRequest).GetProperties())
//{
//    sb.Append($"\t {prop.Name}: {request.GetType().GetProperty(prop.Name).GetValue(request)}\n");
//}

//_logger.Log(LogLevel.Information, $"Handling {typeof(TRequest).Name} \n {sb.ToString()}");
//var response = await next();
//_logger.Log(LogLevel.Information, $"Handled {typeof(TRequest).Name}");
//return response;