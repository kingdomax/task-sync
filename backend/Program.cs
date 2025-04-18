using TaskSync.Configurations;
using TaskSync.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container  ----------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureApiVersion();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.RegisterDependecies(builder.Configuration);
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
app.UseMiddleware<RequestTimingMiddleware>();  // custom middle ware
// -----------------------------------------------------------------------

app.Run();