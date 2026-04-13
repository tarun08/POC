using MassTransit;
using MassTransitPOC.Contracts;

namespace MassTransitPOC.Consumers;

public class OrdersConsumer : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        Console.WriteLine($"Received Order: {context.Message.OrderId} for {context.Message.ItemName} at {context.Message.Timestamp}");
        return Task.CompletedTask;
    }
}
