using Microsoft.AspNetCore.SignalR;
using SocketApp;
using SocketApp.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalRCore();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitMQConsumer>();
builder.Services.AddSingleton<StockHub>();
//builder.Services.AddSingleton<IHubContext<StockHub>>();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
        }));

var app = builder.Build();

//app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
//app.UseCors(config => config.WithOrigins("http://localhost:4200"));
app.UseCors("CorsPolicy");

app.MapHub<StockHub>("/stocksocket");

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRabbitListener();

//RabbitMQFacade.ReceiveMessage((data) => (new StockHub()).BotSending(data), "StockBotQueue");
app.Run();
