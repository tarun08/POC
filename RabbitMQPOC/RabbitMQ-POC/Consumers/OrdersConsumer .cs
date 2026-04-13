using RabbitMQPOC.QueueBase;
using RabbitMQPOC.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQPOC.Consumers
{
    public class OrdersConsumer : RabbitMqConsumerBase
    {
        public OrdersConsumer(IRabbitMqConnection conn) : base(conn) { }

        protected override string QueueName => "orders.queue";

        protected override Task HandleMessage(string message)
        {
            Console.WriteLine($"ORDER: {message}");
            return Task.CompletedTask;
        }
    }
}
