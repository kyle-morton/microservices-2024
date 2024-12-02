using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Domain;
using PlatformService.Models;
using PlatformService.Repos;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
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
    public ActionResult<PlatformRead> CreatePlatform(PlatformCreate model)
    {
        var platformToCreate = _mapper.Map<Platform>(model);
        _repo.Create(platformToCreate);
        var success = _repo.SaveChanges();

        var platformRead = _mapper.Map<PlatformRead>(platformToCreate);

        return CreatedAtAction(nameof(GetPlatform), new { id = platformRead.Id }, platformRead);
    }
}