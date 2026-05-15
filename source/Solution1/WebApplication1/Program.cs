using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using DirectoryService.WebApi;
using Inrstructure.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using WebApplication1.Storage;

Storage.InitializeStorage();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Directory Service API",
            Version = "v1",
            Description = "API для тестирования backend логики",
        }
    );
});

builder.Services.AddControllers();
builder
    .Services.AddOptions<PostgresSettings>()
    .Bind(builder.Configuration.GetSection(nameof(PostgresSettings)));
builder.Services.AddScoped<ApplicationDbContext>();
var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory Service API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public sealed record CreateLocationRequest(string Address, string Name, string TimeZone);

public sealed record CreatePositionRequest(string Name, string Description, bool IsActive);

public sealed record UpdateLocationRequest(
    string? Address = null,
    string? Name = null,
    string? TimeZone = null
);

public sealed record UpdatePositionRequest(
    string? Name = null,
    string? Description = null,
    bool? IsActive = null
);
