using System.ComponentModel.DataAnnotations;
using CoreApi;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {

    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// How to handle exceptions in a common / universal way
app.UseExceptionHandler(cfg =>
{
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#exception-handler-lambda
    cfg.Run(async cxt =>
    {
        cxt.Response.StatusCode = 500;
        cxt.Response.ContentType = "application/json";

        if (cxt.Features.Get<IExceptionHandlerFeature>() is { } ehf)
        {
            // exceptions as control flow
            if (ehf.Error is NotFoundException nfe)
            {
                cxt.Response.StatusCode = 404;
                await cxt.Response.WriteAsJsonAsync(new { a = "NOPE" });
            }
            else if (ehf.Error is { } ex)
            {
                await cxt.Response.WriteAsJsonAsync(new { a = ex.Message ?? "huh" });
            }
            else
            {
                await cxt.Response.WriteAsJsonAsync(new { a = "huh" });
            }
        }
    });
});

app.MapControllers();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/abc", () => "ABC");

// typed results plays with Swagger better
app.MapPost("/invalid", (InvalidRequest request) => TypedResults.Json(request));

app.MapGet("/def", () => "DEF");
app.MapGet("/error", () =>
{
    throw new NotFoundException();

    return Results.Ok();
});

app.MapGet("/problem", () =>
{
    return TypedResults.Problem(new NotFoundProblem());
});

app.Run();


// ReSharper disable once ClassNeverInstantiated.Global
public record InvalidRequest([Required]string Id);
