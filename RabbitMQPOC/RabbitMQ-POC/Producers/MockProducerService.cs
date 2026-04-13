using Microsoft.Extensions.Hosting;
using RabbitMQPOC.QueueBase;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQPOC.Producers
{
    public class MockProducerService : BackgroundService
    {
        private readonly RabbitMqProducer _producer;

        public MockProducerService(RabbitMqProducer producer)
        {
            _producer = producer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var random = new Random();
            var queues = new[] { "orders.queue", "email.queue", "payments.queue" };

            while (!stoppingToken.IsCancellationRequested)
            {
                var randomNumber = random.Next();
                var randomQueue = queues[random.Next(queues.Length)];
                
                await _producer.PublishAsync(randomNumber, randomQueue);

                System.Console.WriteLine($"Published {randomNumber} to {randomQueue}");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
