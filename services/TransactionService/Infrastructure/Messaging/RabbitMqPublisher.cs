using System.Text;
using RabbitMQ.Client;

namespace TransactionService.Infrastructure.Messaging;

public class RabbitMqPublisher
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Publish(string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var queueName = _configuration["RabbitMQ:QueueName"] ?? "transaction.created";
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

        _logger.LogInformation("Message published to RabbitMQ: {Message}", message);
    }
}
