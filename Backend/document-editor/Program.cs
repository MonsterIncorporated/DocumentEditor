var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://document-editor.monsterluni.ch");
        });
    options.AddPolicy(name: "Testing",
        policy =>
        {
            policy.AllowAnyOrigin();
        });
});

var app = builder.Build();

Console.WriteLine($"Current Environment: {app.Environment.EnvironmentName}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}
if (app.Environment.IsStaging())
{
    app.UseCors("Testing");
}

app.MapControllers();
app.UseOpenApi();
app.UseSwaggerUi();
app.UseHttpsRedirection();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();