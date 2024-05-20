using System.Text.Json;
using System.Windows.Input;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.OpenApi.Models;

namespace CommandsService.EventProcessing{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType){
                case EventType.PLatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;

            }        
        }
        private EventType DetermineEvent(string noticationMessage){
            System.Console.WriteLine("-->Determine Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(noticationMessage);

            switch(eventType.Event){
                case "Platform_Published":
                    System.Console.WriteLine("Platform Published Event Detected");
                    return EventType.PLatformPublished;
                default:
                    System.Console.WriteLine("Couldnt determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void addPlatform(string platformPublishedMessage){
            using(var scope = _scopeFactory.CreateScope()){
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExist(plat.ExternalID)){
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        System.Console.WriteLine("Platrform added");
                    }
                    else{
                        System.Console.WriteLine("--> Platform already exists");
                    }
                }
                catch (System.Exception ex)
                {
                    
                    System.Console.WriteLine($"--> Couldnt add Platform to DB{ex.Message}");
                }
            }
        }
    }
    enum EventType{
        PLatformPublished,
        Undetermined
    }
}