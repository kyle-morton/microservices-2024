using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase 
{
    private readonly ICommandRepo _repo;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }    

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadModel>> GetPlatforms() 
    {
        Console.WriteLine("PlatformsController:  Getting Platforms");

        var platforms = _repo.GetAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadModel>>(platforms));
    } 

    [HttpPost]
    public ActionResult TestInboundConnection() 
    {
        Console.WriteLine("PlatformsController:  Inbound Post...");

        return Ok("Inbound test ok.");
    }
}