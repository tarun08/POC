using MassTransit;
using MassTransitPOC.Producers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    // Automatically adds any 'IConsumer<T>' implementations it finds
    x.AddConsumers(Assembly.GetExecutingAssembly());

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => { 
            h.Username("guest"); 
            h.Password("guest"); 
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHostedService<MockProducerService>();

var host = builder.Build();
host.Run();
