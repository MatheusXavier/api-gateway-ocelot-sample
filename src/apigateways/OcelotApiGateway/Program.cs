using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot();

builder.Configuration.AddJsonFile("ocelot.json");

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//app.Run();

await app.UseOcelot();
await app.RunAsync();