using System;
using Microsoft.Extensions.Logging;
using Sentry;
using Sentry.Protocol;

namespace Firepuma.Api.Common.Errors
{
    public class SentryErrorReportingService : IErrorReportingService
    {
        private readonly ILogger<SentryErrorReportingService> _logger;

        public SentryErrorReportingService(ILogger<SentryErrorReportingService> logger)
        {
            _logger = logger;
        }

        public void CaptureException(Exception exception)
        {
            _logger.LogError(exception, "Exception will be reported to sentry");
            SentrySdk.CaptureException(exception);
        }

        public void CaptureWarning(string warningMsg)
        {
            _logger.LogWarning($"Warning message will be reported to sentry: {warningMsg}");
            SentrySdk.CaptureMessage(warningMsg, SentryLevel.Warning);
        }
    }
}