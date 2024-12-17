using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.EventProcessing;
using RabbitMQ.Client;

namespace CommandsService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
    {
        _config = config;
        _eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ() {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQHost"],
            Port = int.Parse(_config["RabbitMQPort"])
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(
            queue: _queueName,
            exchange: "trigger",
            routingKey: ""
        );
        Console.WriteLine("MessageBusSubscriber: Listening on message bus...");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("MessageBusSubscriber: Connection shutdown...");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
        if (_channel.IsOpen) {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }
}
