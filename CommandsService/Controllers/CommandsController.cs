using AutoMapper;
using CommandsService.Data;
using CommandsService.Domain;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadModel>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadModel>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadModel> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(platformId, commandId);

            if(command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadModel>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadModel> CreateCommandForPlatform(int platformId, CommandCreateModel commandModel)
        {
             Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandModel);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadModel = _mapper.Map<CommandReadModel>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new {platformId = platformId, commandId = commandReadModel.Id}, commandReadModel);
        }

    }
}