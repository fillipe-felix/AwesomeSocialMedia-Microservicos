using System.Text;

using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AwesomeSocialMedia.Newsfeed.API.Consumers;

public class UserUpdatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;
    private const string Queue = "newsfeed.user-updated";
    private const string Exchange = "user";
    private const string RoutingKey = "user-updated";

    public UserUpdatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // Configurando conexão com RabbitMQ
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection("newsfeed.user-updated");

        _channel = connection.CreateModel();

        // Garantir que Exchange e Fila estão criados, e fazer o Binding entre eles
        _channel.QueueDeclare(Queue, true, false, false, null);

        _channel.ExchangeDeclare(Exchange, "direct", true, false);

        _channel.QueueBind(Queue, Exchange, RoutingKey);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(contentArray);

            var @event = JsonConvert.DeserializeObject<UpdateUser>(json);

            Console.WriteLine(json);

            await UpdateUser(@event);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }
    
    private async Task UpdateUser(UpdateUser @event)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserNewsfeedRepository>();
            
            var user = await repository.GetByUserId(@event.Id);

            user.User.DisplayName = @event.DisplayName;

            await repository.Updated(user);
        }
    }
}

public class UpdateUser
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
}
