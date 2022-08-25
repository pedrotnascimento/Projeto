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
        }

        public async Task BotSending(string message)
        {
            context.Clients.All.SendAsync("stockCommandReceived", message);
        }
    }

}

