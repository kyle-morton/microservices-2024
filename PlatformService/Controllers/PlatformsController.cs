using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Domain;
using PlatformService.Models;
using PlatformService.Repos;
using PlatformService.DataServices.Sync.Http;
using PlatformService.DataServices.Async;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
        IPlatformRepo repo, 
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repo = repo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<List<PlatformReadModel>> GetPlatforms()
    {
        var platforms = _repo.Get();
        var models = _mapper.Map<List<PlatformReadModel>>(platforms);

        return Ok(models);
    }

    [HttpGet("{id}")]
    public ActionResult<PlatformReadModel> GetPlatform(int id)
    {
        var platform = _repo.Get(id);

        if (platform == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PlatformReadModel>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadModel>> CreatePlatform(PlatformCreateModel model)
    {
        var platformToCreate = _mapper.Map<Platform>(model);
        _repo.Create(platformToCreate);
        var success = _repo.SaveChanges();

        var platformRead = _mapper.Map<PlatformReadModel>(platformToCreate);

        // Send SYNC Message
        try 
        {
            await _commandDataClient.SendPlatformToCommand(platformRead);
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"PlatformsController: Could not send synchronously {ex}");
        }

        // Send Async Message
        try 
        {
            var publishModel = _mapper.Map<PlatformPublishedModel>(platformRead);
            publishModel.Event = "Platform_Published"; // event that our consumer will need to listen for
            _messageBusClient.PublishNewPlatform(publishModel);
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"PlatformsController: Could not send synchronously {ex}");
        }

        return CreatedAtAction(nameof(GetPlatform), new { id = platformRead.Id }, platformRead);
    }
}