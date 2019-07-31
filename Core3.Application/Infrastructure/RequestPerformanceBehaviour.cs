using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core3.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core3.Application.Infrastructure
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger<TRequest> _logger;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            Guard.ArgumentNotNull(logger, nameof(logger));

            _stopwatch = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _stopwatch.Start();

            TResponse response = await next();

            _stopwatch.Stop();

            if (_stopwatch.ElapsedMilliseconds > 500)
            {
                string name = typeof(TRequest).Name;

                _logger.LogError($"Request: {name} ({_stopwatch.ElapsedMilliseconds}) {request}");
            }

            return response;
        }
    }
}
