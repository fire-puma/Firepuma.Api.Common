using Sentry.AspNetCore;

// ReSharper disable LocalizableElement

namespace Firepuma.Api.Common.Configure
{
    public static class ErrorReportingConfigHelper
    {
        public static void ConfigureSentry(SentryAspNetCoreOptions cfg, string release, bool attachStackTrace = true)
        {
            cfg.Release = release;
            cfg.AttachStacktrace = attachStackTrace;
        }
    }
}
