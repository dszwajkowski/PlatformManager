using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.MessageBus;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        int port;
        if (!int.TryParse(_configuration["RabbitMQPort"], out port))
        {
            throw new NullReferenceException("RabbitMQ port");
        }

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"],
            Port = port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        Console.WriteLine("Connected to RabbitMQ.");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        if (_connection.IsOpen) 
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            SendMessage(message);
        }
    }

    private void SendMessage(string message) 
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("trigger", string.Empty, null, body);
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ connection shutdown.");
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
