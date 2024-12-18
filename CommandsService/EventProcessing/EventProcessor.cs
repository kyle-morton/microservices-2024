using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Domain;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.EventProcessing;

public interface IEventProcessor
{
    void Process(string message);
}

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }


    public void Process(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            default:
                break;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedModel = JsonSerializer.Deserialize<PlatformPublishedModel>(platformPublishedMessage);

        try
        {
            var plat = _mapper.Map<Platform>(platformPublishedModel);

            if (!repo.ExternalPlatformExists(plat.ExternalId))
            {
                Console.WriteLine($"AddPlatform: Platform saving to db - {plat.ExternalId}");

                repo.CreatePlatform(plat);
                repo.SaveChanges();

                Console.WriteLine($"AddPlatform: Platform saved successfully to db - {plat.ExternalId}");
            }
            else
            {
                Console.WriteLine($"AddPlatform: Platform already exists - {plat.ExternalId}");
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddPlatform: could not add platform to db {ex.Message}");
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("Determine Event: Starting...");

        var eventType = JsonSerializer.Deserialize<GenericEvent>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("Determine Event: Platform published!");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("Determine Event: Error - Could not determine type!");
                return EventType.Undetermined;
        }
    }
}

public enum EventType
{
    Undetermined = 0,
    PlatformPublished = 1,
}