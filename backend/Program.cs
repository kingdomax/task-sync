using System.Text.Json.Serialization;

using OpenTelemetry.Metrics;

using TaskSync.Infrastructure.Configurations;
using TaskSync.MiddleWares;
using TaskSync.SignalR;

// ------------------------ Setup all services  ----------------------------------
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR().AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddRateLimiter();
builder.Services.AddMemoryCache(options => options.SizeLimit = 100);
builder.Services.ConfigureAppSettings(builder.Configuration);
builder.Services.ConfigureApiVersion();
builder.Services.ConfigureResponseCompression();
builder.Services.ConfigureCors();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureDependencyInjection(builder.Configuration);
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation(); // .AddPrometheusExporter();
    });
// -------------------------------------------------------------------------------

// -------------- Configure/Order the request pipeline (Middleware) --------------
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0#middleware-order
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStaticFiles(); // Required for serving CSS, JS, etc.
app.UseCors();
app.UseResponseCompression();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();
app.MapControllers(); // or MapControllerRoute, inside it call UseRouting()
app.MapHub<TaskHub>("/taskHub");
// app.UseOpenTelemetryPrometheusScrapingEndpoint(); // exposes /metrics
app.Run();
// -----------------------------------------------------------------------
