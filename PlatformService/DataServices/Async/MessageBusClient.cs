using System;
using System.Text;
using System.Text.Json;
using PlatformService.Models;
using RabbitMQ.Client;


namespace PlatformService.DataServices.Async;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedModel model);
}

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection? _connection;
    private readonly IModel? _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine("MessageBusClient: Connected to MessageBus");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"MessageBusClient: Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishedModel platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("MessageBusClient: RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("MessageBusClient: RabbitMQ connectionis closed, not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "trigger",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
        Console.WriteLine($"MessageBusClient: We have sent {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("MessageBus Disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("MessageBusClient: RabbitMQ Connection Shutdown");
    }
}

