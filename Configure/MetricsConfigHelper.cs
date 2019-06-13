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
        public static InfluxConfig LoadInfluxMetricsConfigFromEnv(bool requireEnv)
        {
            var url = Environment.GetEnvironmentVariable("INFLUX_METRICS_URL");
            var database = Environment.GetEnvironmentVariable("INFLUX_METRICS_DATABASE");
            var username = Environment.GetEnvironmentVariable("INFLUX_METRICS_USERNAME");
            var password = Environment.GetEnvironmentVariable("INFLUX_METRICS_PASSWORD");

            if (requireEnv && string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("INFLUX_METRICS_URL environment variable is required but missing");
            }

            if (requireEnv && string.IsNullOrWhiteSpace(database))
            {
                throw new Exception("INFLUX_METRICS_DATABASE environment variable is required but missing");
            }

            if (requireEnv && string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("INFLUX_METRICS_USERNAME environment variable is required but missing");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("WARNING: INFLUX_METRICS_URL environment variable is empty, continuing without it");

                return null;
            }

            Console.WriteLine("Loaded INFLUX_METRICS_URL environment variable");

            return new InfluxConfig(url, database, username, password);
        }

        public static void ConfigureMetrics(IMetricsBuilder builder, InfluxConfig influxConfig, string appName)
        {
            builder.Report.ToInfluxDb(options =>
            {
                options.InfluxDb.BaseUri = new Uri(influxConfig.Url);
                options.InfluxDb.Database = influxConfig.Database;
                options.InfluxDb.UserName = influxConfig.Username;
                options.InfluxDb.Password = influxConfig.Password;
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