using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CommandsService.Data;
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

    [HttpPost]
    public ActionResult TestInboundConnection() 
    {
        Console.WriteLine("--> Inbound Post...");

        return Ok("Inbound test ok.");
    }
}