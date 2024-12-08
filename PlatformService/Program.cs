using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Repos;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseInMemoryDatabase("InMemoryDB");
});

// Services
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

// Register Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// skipping https since it makes K8S setup harder (needed but not for this tutorial)
// app.UseHttpsRedirection();

// autopopulate db
PrepDb.PopulateDb(app);

Console.WriteLine();
Console.WriteLine($"PlatformService: CommandService Endpoint {app.Configuration["CommandService"]}");
Console.WriteLine();

app.Run();