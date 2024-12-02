using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Repos;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseInMemoryDatabase("InMemoryDB");
});

// Services
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// autopopulate db
PrepDb.PopulateDb(app);

app.Run();