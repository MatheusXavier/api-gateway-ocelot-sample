using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Configuration.AddJsonFile("ocelot.json");

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSwaggerForOcelotUI(option =>
{
    option.PathToSwaggerGenerator = "/swagger/docs";
});
app.MapControllers();

await app.UseOcelot();
await app.RunAsync();