using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace PoemGenerator.Monolith.Notifications;

public class RabbitMqNotificationService : INotificationService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName = "poem.notifications";

    public RabbitMqNotificationService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq", // change if your service name is different
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Fanout, durable: true).Wait();
    }

    public async Task NotifyPoemAddedAsync(int id, string title, string author)
    {
        var notification = new PoemAddedNotification
        {
            Id = id,
            Title = title,
            Author = author
        };

        var message = JsonSerializer.Serialize(notification);
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(_exchangeName, "", body);
    }
}
