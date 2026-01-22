using MyProjectMaybe.Infrastructure;
using MyProjectMaybe.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUploadStore, InMemoryUploadStore>();

builder.Services.AddHostedService<ValidationWorker>();

builder.Services.AddHostedService<ProcessingWorker>();

var app = builder.Build();

app.MapPost("/uploads", (IUploadStore store) =>
{
    var upload = store.Create();

    return Results.Ok(new
    {
        upload.Id,
        upload.State
    });
});

app.MapGet("/", () => "Hello World!");

app.Run();
