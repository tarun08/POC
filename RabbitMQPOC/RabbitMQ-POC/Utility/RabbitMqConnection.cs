using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQPOC.Utility
{
    public interface IRabbitMqConnection
    {
        Task<IConnection> CreateConnectionAsync();
    }

    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMqOptions _options;
        private readonly ConnectionFactory _factory;

        public RabbitMqConnection(IConfiguration config)
        {
            _options = config.GetSection("RabbitMQ").Get<RabbitMqOptions>()!;

            _factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };
        }

        public Task<IConnection> CreateConnectionAsync()
        {
            return _factory.CreateConnectionAsync();
        }
    }
}
