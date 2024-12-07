using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Domain;
using PlatformService.Models;
using PlatformService.Repos;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(
        IPlatformRepo repo, 
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _repo = repo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<List<PlatformRead>> GetPlatforms()
    {
        var platforms = _repo.Get();
        var models = _mapper.Map<List<PlatformRead>>(platforms);

        return Ok(models);
    }

    [HttpGet("{id}")]
    public ActionResult<PlatformRead> GetPlatform(int id)
    {
        var platform = _repo.Get(id);

        if (platform == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformRead>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformRead>> CreatePlatform(PlatformCreate model)
    {
        var platformToCreate = _mapper.Map<Platform>(model);
        _repo.Create(platformToCreate);
        var success = _repo.SaveChanges();

        var platformRead = _mapper.Map<PlatformRead>(platformToCreate);

        try 
        {
            await _commandDataClient.SendPlatformToCommand(platformRead);
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"PlatformsController: Could not send synchronously {ex}");
        }

        return CreatedAtAction(nameof(GetPlatform), new { id = platformRead.Id }, platformRead);
    }
}