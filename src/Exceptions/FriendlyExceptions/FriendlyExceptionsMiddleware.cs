using System;
using System.Threading.Tasks;
using Firepuma.Api.Abstractions.Errors;
using Firepuma.Api.Common.Exceptions.FriendlyExceptions.Extensions;
using Firepuma.Api.Common.Exceptions.FriendlyExceptions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions
{
    internal class FriendlyExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<TranformOptions> _options;
        private readonly ILogger<FriendlyExceptionsMiddleware> _logger;
        private readonly IErrorReportingService _errorReportingService;

        public FriendlyExceptionsMiddleware(
            RequestDelegate next,
            IOptions<TranformOptions> options,
            ILogger<FriendlyExceptionsMiddleware> logger,
            IErrorReportingService errorReportingService)
        {
            _next = next;
            _options = options;
            _logger = logger;
            _errorReportingService = errorReportingService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _errorReportingService.CaptureException(exception);

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the FriendlyExceptionsMiddleware will not be executed.");
                    throw;
                }

                await context.HandleExceptionAsync(_options, exception);
            }
        }
    }
}