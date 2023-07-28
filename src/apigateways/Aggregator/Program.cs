using Aggregator.Services;

using BasketApi;

using ProductApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IBasketService, BasketService>();

builder.Services.AddGrpcClient<ProductGrpc.ProductGrpcClient>((services, options) =>
{
    options.Address = new Uri("https://localhost:7087");
});

builder.Services.AddGrpcClient<BasketGrpc.BasketGrpcClient>((services, options) =>
{
    options.Address = new Uri("https://localhost:7298");
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
