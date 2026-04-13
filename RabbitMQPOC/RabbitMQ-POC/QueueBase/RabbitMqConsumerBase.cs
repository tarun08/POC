using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQPOC.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQPOC.QueueBase
{
    public abstract class RabbitMqConsumerBase : BackgroundService
    {
        protected abstract string QueueName { get; }
        protected abstract Task HandleMessage(string message);

        private IConnection? _conn;
        private IChannel? _channel;
        private readonly IRabbitMqConnection _connection;

        protected RabbitMqConsumerBase(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _conn = await _connection.CreateConnectionAsync();
            _channel = await _conn.CreateChannelAsync();

            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

            await _channel.BasicQosAsync(0, 1, false);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (s, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body.ToArray());

                try
                {
                    await HandleMessage(msg);
                    await _channel!.BasicAckAsync(e.DeliveryTag, false);
                }
                catch
                {
                    await _channel!.BasicNackAsync(e.DeliveryTag, false, false);
                }
            };

            await _channel!.BasicConsumeAsync(QueueName, false, consumer);
        }
    }
}
