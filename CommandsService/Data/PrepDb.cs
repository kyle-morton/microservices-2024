using CommandsService.Domain;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PopulateDb(WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
        var platforms = grpcClient.ReturnAllPlatforms();

        var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();

        Seed(repo, platforms);
    }

    private static void Seed(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("PrepDB: Seeding new platforms...");

        foreach(var plat in platforms) {
            if (!repo.PlatformExists(plat.ExternalId)) {
                repo.CreatePlatform(plat);
                repo.SaveChanges();
            }
        }
    }
}