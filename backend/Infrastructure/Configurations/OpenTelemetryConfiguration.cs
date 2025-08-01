using Microsoft.Extensions.Options;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using TaskSync.Infrastructure.Settings;

namespace TaskSync.Infrastructure.Configurations
{
    public static class OpenTelemetryConfiguration
    {
        public static void ConfigureTelemetry(this WebApplicationBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            var appInfo = provider.GetRequiredService<IOptions<AppInfo>>().Value;
            var otelSettings = provider.GetRequiredService<IOptions<OtelCollectorSettings>>().Value;

            // Shared resource attributes (customize for app)
            var otelResource = ResourceBuilder.CreateDefault().AddService(appInfo.AppName, serviceVersion: "1.0.0");

            // Add OpenTelemetry for Tracing and Metrics
            builder.Services.AddOpenTelemetry().ConfigureResource(rb => rb.AddService(appInfo.AppName))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = new Uri(otelSettings.BaseUrl); // http://otel-collector:4317 (grpc protocol)
                            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                })
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation() // automatic telemetry for incoming HTTP requests e.g. "/api/projects/1/tasks"
                        .AddHttpClientInstrumentation() // automatic telemetry for outgoing HTTP requests e.g. HttpClient usage
                        .AddRuntimeInstrumentation() // Garbage collection counts, Thread pool usage, Memory pressure
                        .AddProcessInstrumentation() // CPU usage, Memory, Thread count
                        .AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = new Uri(otelSettings.BaseUrl);
                            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                });

            // Add OpenTelemetry Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(loggerOptions =>
            {
                loggerOptions.IncludeScopes = true;
                loggerOptions.IncludeFormattedMessage = true;
                loggerOptions.ParseStateValues = true;

                loggerOptions.SetResourceBuilder(otelResource);

                loggerOptions.AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri(otelSettings.BaseUrl);
                });
            });
        }
    }
}
