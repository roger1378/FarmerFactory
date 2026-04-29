using FarmerFactory.Api.Configuration;
using FarmerFactory.Common.Exceptions;
using FarmerFactory.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services
    .AddMapperServices()
    .AddMyDependencyGroup()
    .AddOpenApi();

// Use in-memory SQLite database
builder.Services.AddDbContext<CoreApiDbContext>(options =>
    options.UseSqlite("DataSource=FarmerFactory.db"));

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Ensure the database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CoreApiDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add exception handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandler?.Error is TooManyRequestsException)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsJsonAsync(new { message = "Too many requests" });
        }
        else if (exceptionHandler?.Error != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                message = "An error occurred",
                error = app.Environment.IsDevelopment() ? exceptionHandler.Error.Message : null
            });
        }
    });
});

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
