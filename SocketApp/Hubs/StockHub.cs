using Microsoft.AspNetCore.SignalR;

namespace SocketApp.Hubs
{
    public class StockHub : Hub
    {
        private IHubContext<StockHub> context;

        public StockHub(
            IHubContext<StockHub> context) : base()
        {
            this.context = context;
            //RabbitMQFacade.ReceiveMessage(BotSending, "StockBotQueue");
        }

        public async Task BotSending(string message)
        {
            //Console.WriteLine($"{obj}");
            context.Clients.All.SendAsync("messageReceived", message);
            //await Clients.All.SendAsync("messageReceived", message);
        }
    }

}

