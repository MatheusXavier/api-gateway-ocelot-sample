using Basket.API.Grpc;
using Basket.API.Infrastructure.Repositories;

using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
    string connectionString = builder.Configuration["ConnectionString"] ?? string.Empty;

    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddTransient<IBasketRepository, RedisBasketRepository>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<BasketService>();

app.Run();
