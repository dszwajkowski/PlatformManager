using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.EventProcessing;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;

        InitializeMessageBus();
    }

    private void InitializeMessageBus()
    {
        int port;
        if (!int.TryParse(_configuration["RabbitMQPort"], out port))
        {
            throw new NullReferenceException("RabbitMQ port");
        }


        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = port,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, "trigger", "");

        Console.WriteLine("Listening to MessageBus.");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ connection shutdown.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("Event received.");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(_queueName, true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }
}
