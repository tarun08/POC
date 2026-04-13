using MassTransit;
using MassTransitPOC.Contracts;

namespace MassTransitPOC.Consumers;

public class EmailConsumer : IConsumer<EmailNotificationRequest>
{
    public Task Consume(ConsumeContext<EmailNotificationRequest> context)
    {
        Console.WriteLine($"Received Email Request for user: {context.Message.UserId} to address {context.Message.EmailAddress}");
        return Task.CompletedTask;
    }
}
