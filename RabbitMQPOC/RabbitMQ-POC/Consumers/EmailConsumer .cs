using RabbitMQPOC.QueueBase;
using RabbitMQPOC.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQPOC.Consumers
{
    public class EmailConsumer : RabbitMqConsumerBase
    {
        public EmailConsumer(IRabbitMqConnection conn) : base(conn) { }

        protected override string QueueName => "email.queue";

        protected override Task HandleMessage(string message)
        {
            Console.WriteLine($"EMAIL: {message}");
            return Task.CompletedTask;
        }
    }
}
