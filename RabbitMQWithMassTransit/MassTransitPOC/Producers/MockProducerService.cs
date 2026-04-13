using MassTransit;
using MassTransitPOC.Contracts;
using Microsoft.Extensions.Hosting;

namespace MassTransitPOC.Producers;

public class MockProducerService : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MockProducerService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int count = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            count++;
            
            var order = new OrderCreated(count, $"Item-{count}", DateTime.UtcNow);
            await _publishEndpoint.Publish(order, stoppingToken);
            Console.WriteLine($"[Producer] Published OrderCreated: {order.OrderId}");

            var email = new EmailNotificationRequest(count, $"user{count}@example.com");
            await _publishEndpoint.Publish(email, stoppingToken);
            Console.WriteLine($"[Producer] Published EmailNotificationRequest: {email.UserId}");

            await Task.Delay(5000, stoppingToken);
        }
    }
}
