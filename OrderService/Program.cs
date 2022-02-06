using MassTransit;
using RabbitMQ.Client;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Send<Order>(x =>
        {
            // use customerType for the routing key
            x.UseRoutingKeyFormatter(context => context.Message.OrderType.ToString());
        });

        cfg.Message<Order>(x => x.SetEntityName("entity.name.order"));

        cfg.Publish<Order>(x => x.ExchangeType = ExchangeType.Direct);
    });
});

//builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();