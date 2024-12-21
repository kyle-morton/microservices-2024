using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.DataServices.Async;
using PlatformService.DataServices.Sync.Grpc;
using PlatformService.DataServices.Sync.Http;
using PlatformService.Repos;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("PlatformService -> " + builder.Environment.EnvironmentName);

// DbContext - memory if not prod, sql server if prod
if (!builder.Environment.IsProduction()) 
{
    Console.WriteLine("PlatformService -> Using in memory db");
    builder.Services.AddDbContext<AppDbContext>(options => {
        options.UseInMemoryDatabase("InMemoryDB");
    });
}
else 
{
    var connString = builder.Configuration.GetConnectionString("PlatformConnStr");
    Console.WriteLine("PlatformService -> Using in sql server - " + connString);

    builder.Services.AddDbContext<AppDbContext>(options => {
        options.UseSqlServer(connString);
    });
}


// Services
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

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
app.MapGrpcService<GrpcPlatformService>();

app.MapGet("/protos/platforms.proto", async context => {
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

// skipping https since it makes K8S setup harder (needed but not for this tutorial)
// app.UseHttpsRedirection();

// autopopulate db
PrepDb.PopulateDb(app);

Console.WriteLine();
Console.WriteLine($"PlatformService: CommandService Endpoint {app.Configuration["CommandService"]}");
Console.WriteLine();

app.Run();