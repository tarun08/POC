using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQPOC.Consumers;
using RabbitMQPOC.Producers;
using RabbitMQPOC.QueueBase;
using RabbitMQPOC.Utility;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<RabbitMqProducer>();

builder.Services.AddHostedService<OrdersConsumer>();
builder.Services.AddHostedService<EmailConsumer>();

builder.Services.AddHostedService<MockProducerService>();

var host = builder.Build();
host.Run();