using EventBus.Messages.Common;
using MassTransit;
using Ordering.Data;
using Ordering.Dispatcher;
using Ordering.EventBusConsumer;
using Ordering.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfraServices(builder.Configuration);

builder.Services.AddHostedService<OutboxMessageDispatcher>();

// Mass Transit
builder.Services.AddMassTransit(config =>
{
    // Mark as consumer
    config.AddConsumer<BasketOrderingConsumer>();
    config.AddConsumer<PaymentCompletedConsumer>();
    config.AddConsumer<PaymentFailedConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        // provide the queue name with consumer settings
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(EventBusConstant.PaymentCompletedQueue, e =>
        {
            e.ConfigureConsumer<PaymentCompletedConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(EventBusConstant.PaymentFailedQueue, e =>
        {
            e.ConfigureConsumer<PaymentFailedConsumer>(ctx);
        });
    });
});

var app = builder.Build();

app.MigrateDabase<OrderContext>((context,services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context,logger).Wait();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
