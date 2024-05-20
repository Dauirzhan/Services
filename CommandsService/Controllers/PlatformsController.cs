using System.Windows.Input;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{ 
    [Route("api/c/platforms")]
    [ApiController]
    public class PlatformsController : ControllerBase{
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms(){
            System.Console.WriteLine("Getting Platforms for CommandService");

            var platformitems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformitems));
        }


        [HttpPost]
        public ActionResult Test()
        {
            System.Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("test Platforms Controller");
        }
    }
}