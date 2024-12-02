using PlatformService.Domain;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PopulateDb(WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        Seed(context);
    }

    private static void Seed(AppDbContext context)
    {
        if (context == null)
        {
            Console.WriteLine("PrepDb: context null");
            return;
        }
        if (context.Platforms.Any())
        {
            Console.WriteLine("PrepDb: Data exists, skipping populate");
            return;
        }

        Console.WriteLine("PrepDb: seeding data");
        context.Platforms.AddRange(new List<Platform> {
            new Platform { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
            new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
            new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
        });

        context.SaveChanges();
    }
}