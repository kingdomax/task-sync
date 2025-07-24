using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TaskSync.Infrastructure.Configurations
{
    // Note: The OpenTelemetry collector should be running in the same Docker network as the API service.
    public static class OpenTelemetryConfiguration
    {
        public static void ConfigureOpenTelemetry(this WebApplicationBuilder builder)
        {
            // Shared resource attributes (customize for app)
            var otelResource = ResourceBuilder.CreateDefault().AddService("Core.API", serviceVersion: "1.0.0");

            // Add OpenTelemetry for Tracing and Metrics
            builder.Services.AddOpenTelemetry().ConfigureResource(rb => rb.AddService("Core.API")) // todo-moch: don't hardcode this
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddOtlpExporter(opt =>
                        {
                            opt.Endpoint = new Uri("http://otel-collector:4317"); // Use Docker service name! and todo-moch: don't hardcode this
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
                            opt.Endpoint = new Uri("http://otel-collector:4317");
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
                    otlpOptions.Endpoint = new Uri("http://otel-collector:4317"); // or 4318 for HTTP
                });
            });
        }
    }
}
