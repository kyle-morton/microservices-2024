using Microsoft.EntityFrameworkCore;
using PlatformService.Domain;

namespace PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Platform> Platforms {get;set;}
}