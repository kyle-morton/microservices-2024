using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Repos;

public interface IPlatformRepo 
{
    bool SaveChanges(); 
    List<Platform> Get();
    Platform? Get(int id);
    void Create(Platform platform);
}

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
{
        _context = context;
    }

    public void Create(Platform platform)
    {
        _context.Platforms.Add(platform);
    }

    public List<Platform> Get()
    {
        return _context.Platforms.ToList();
    }

    public Platform? Get(int id)
    {
        return _context.Platforms.FirstOrDefault(p => p.Id == id);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }
}