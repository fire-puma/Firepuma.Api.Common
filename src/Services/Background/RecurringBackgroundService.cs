using System;
using System.Threading;
using System.Threading.Tasks;
using Firepuma.Api.Abstractions.Errors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global

namespace Firepuma.Api.Common.Services.Background
{
    public abstract class RecurringBackgroundService : BackgroundService
    {
        private readonly ILogger<RecurringBackgroundService> _logger;
        private readonly IErrorReportingService _errorReportingService;

        protected RecurringBackgroundService(
            ILogger<RecurringBackgroundService> logger,
            IErrorReportingService errorReportingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _errorReportingService = errorReportingService ?? throw new ArgumentNullException(nameof(errorReportingService));
        }

        protected abstract TimeSpan LoopInterval { get; }
        protected abstract Task ExecuteIteration(CancellationToken cancellationToken, int iterationNumber);
        protected abstract bool CanContinueAfterException(Exception exception);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{nameof(RecurringBackgroundService)} is starting.");

            cancellationToken.Register(() => _logger.LogWarning($"{nameof(RecurringBackgroundService)} background task is stopping."));

            int iterationNumber = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{nameof(RecurringBackgroundService)} task doing background work.");

                try
                {
                    iterationNumber++;

                    await ExecuteIteration(cancellationToken, iterationNumber);
                }
                catch (Exception exception)
                {
                    _errorReportingService.CaptureException(exception);

                    var canContinue = CanContinueAfterException(exception);
                    if (!canContinue)
                    {
                        _errorReportingService.CaptureWarning($"{nameof(CanContinueAfterException)} is FALSE so will now stop execution of {nameof(RecurringBackgroundService)}");
                        return;
                    }
                }

                await Task.Delay(LoopInterval, cancellationToken);
            }

            _logger.LogDebug($"{nameof(RecurringBackgroundService)} background task is stopping.");
        }

        protected async Task ExecuteJob(JobContext jobContext, IJob job)
        {
            try
            {
                var reasonToSkip = await job.GetReasonToSkip();
                if (!string.IsNullOrWhiteSpace(reasonToSkip))
                {
                    _logger.LogDebug(reasonToSkip);
                    return;
                }
            }
            catch (Exception exception)
            {
                _errorReportingService.CaptureException(exception);

                var canContinue = CanContinueAfterException(exception);
                if (!canContinue)
                {
                    throw;
                }
            }

            try
            {
                await job.Do(jobContext);
            }
            catch (Exception exception)
            {
                _errorReportingService.CaptureException(exception);

                var canContinue = CanContinueAfterException(exception);
                if (!canContinue)
                {
                    throw;
                }
            }
        }
    }
}