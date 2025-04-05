var builder = WebApplication.CreateBuilder(args);

// -------------- Add services to the container (DI Config) --------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:3039",    // Dev
                "https://tasksync.com"    // Production frontend
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// -----------------------------------------------------------------------

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
// -----------------------------------------------------------------------

app.Run();
