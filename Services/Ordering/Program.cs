using Ordering.Data;
using Ordering.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfraServices(builder.Configuration);

var app = builder.Build();

app.MigrateDabase<OrderContext>((context,services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context,logger).Wait();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
