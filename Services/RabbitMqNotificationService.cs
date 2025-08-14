using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PoemGenerator.Monolith.Configuration;
using RabbitMQ.Client;

namespace PoemGenerator.Monolith.Notifications;

public class RabbitMqNotificationService : INotificationService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName = "poem.notifications";

    public RabbitMqNotificationService(IOptions<RabbitMqOptions> options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            Port = options.Value.Port,
            UserName = options.Value.UserName,
            Password = options.Value.Password
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
