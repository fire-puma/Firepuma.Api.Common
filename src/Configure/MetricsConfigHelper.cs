using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Scheduling;

// ReSharper disable LocalizableElement

namespace Firepuma.Api.Common.Configure
{
    public static class MetricsConfigHelper
    {
        public static void ConfigureMetrics(IMetricsBuilder builder, MetricsInfluxConfig config, string appName)
        {
            builder.Report.ToInfluxDb(options =>
            {
                options.InfluxDb.BaseUri = new Uri(config.Url);
                options.InfluxDb.Database = config.Database;
                options.InfluxDb.UserName = config.Username;
                options.InfluxDb.Password = config.Password;
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                options.Filter = null;
                options.FlushInterval = TimeSpan.FromSeconds(20);
            });

            builder.Configuration.Configure(options =>
            {
                options.GlobalTags["app"] = appName;
                options.Enabled = true;
                options.ReportingEnabled = true;
            });

            var scheduler = new AppMetricsTaskScheduler(
                TimeSpan.FromSeconds(3),
                async () => { await Task.WhenAll(new MetricsBuilder().Build().ReportRunner.RunAllAsync()); });

            scheduler.Start();
        }
    }
}