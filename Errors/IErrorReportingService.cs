using System;

namespace Firepuma.Api.Common.Errors
{
    public interface IErrorReportingService
    {
        void CaptureException(Exception exception);
        void CaptureWarning(string warningMsg);
    }
}
