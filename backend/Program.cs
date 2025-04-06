using TaskSync.Configurations;
using TaskSync.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container  ----------------------------------
builder.Services.AddDiInjections(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:3039",  // Dev
                "https://tasksync.com"    // Production
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// -------------------------------------------------------------------------------

var app = builder.Build();

// -------------- Configure the HTTP request pipeline (Middleware) --------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<RequestTimingMiddleware>();
//app.UseMiddleware<RequestLoggerMiddleware>();
// -----------------------------------------------------------------------

app.Run();