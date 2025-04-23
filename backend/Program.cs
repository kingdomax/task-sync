using System.Text.Json.Serialization;
using TaskSync;
using TaskSync.MiddleWares;
using TaskSync.Configurations;

var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container  ----------------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureApiVersion();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.RegisterDependecies(builder.Configuration);
builder.Services.AddSignalR();
// -------------------------------------------------------------------------------

var app = builder.Build();

// -------------- Configure the HTTP request pipeline (Middleware) --------------
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TaskHub>("/taskHub");
app.UseMiddleware<RequestTimingMiddleware>();  // custom middle ware
// -----------------------------------------------------------------------

app.Run();