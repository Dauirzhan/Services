using System.ComponentModel.Design;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers{
    [Route("api/c/platforms/{platformId}/commands")]
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
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatforms(int platformId){
            System.Console.WriteLine($"Hit CommandsForPlatofrm {platformId}");   

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }     
            var commands = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));        
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId){
           System.Console.WriteLine($"Hit CommandsForPlatofrm {platformId} / {commandId}");   

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = _repository.GetCommand(platformId, commandId);
            if(command == null){
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPaltform(int platformId, CommandCreateDto commandDto){
             System.Console.WriteLine($"Hit CreateCommandsForPlatofrm {platformId}");   

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}