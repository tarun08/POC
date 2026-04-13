using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQPOC.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RabbitMQPOC.QueueBase
{
    public class RabbitMqProducer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly RabbitMqOptions _options;

        public RabbitMqProducer(IRabbitMqConnection connection, IConfiguration config)
        {
            _connection = connection;
            _options = config.GetSection("RabbitMQ").Get<RabbitMqOptions>()!;
        }

        public async Task PublishAsync<T>(T message, string routingKey)
        {
            await using var conn = await _connection.CreateConnectionAsync();
            await using var channel = await conn.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Direct,
                durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var props = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: _options.ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body);
        }
    }
}
