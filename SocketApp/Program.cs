using Microsoft.AspNetCore.SignalR;
using SocketApp;
using SocketApp.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalRCore();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddSingleton<StockHub>();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
        }));

var app = builder.Build();

app.UseCors("CorsPolicy");

app.MapHub<StockHub>("/stocksocket");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRabbitListener();

app.Run();
