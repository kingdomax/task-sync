using System.Text.Json.Serialization;

using TaskSync.Infrastructure.Configurations;
using TaskSync.MiddleWares;
using TaskSync.SignalR;

var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container  ----------------------------------
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
// -------------------------------------------------------------------------------

var app = builder.Build();

// -------------- Configure the HTTP request pipeline (Middleware) --------------
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseStaticFiles(); // Required for serving CSS, JS, etc.
app.UseResponseCompression();
app.MapControllers(); // or MapControllerRoute, inside it call UseRouting()
app.MapHub<TaskHub>("/taskHub");
app.UseMiddleware<ExceptionHandlingMiddleware>(); // custom middlewares
app.UseMiddleware<LoggerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();
// -----------------------------------------------------------------------

app.Run();
