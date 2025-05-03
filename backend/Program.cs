using System.Text.Json.Serialization;
using TaskSync.MiddleWares;
using TaskSync.SignalR;
using TaskSync.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container  ----------------------------------
builder.Services.AddSignalR().AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureApiVersion();
builder.Services.ConfigureCors();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddMemoryCache(options => options.SizeLimit = 100);
builder.Services.RegisterDependecies(builder.Configuration);
// -------------------------------------------------------------------------------

var app = builder.Build();

// -------------- Configure the HTTP request pipeline (Middleware) --------------
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TaskHub>("/taskHub");
app.UseMiddleware<RequestTimingMiddleware>();  // custom middle ware
// -----------------------------------------------------------------------

app.Run();