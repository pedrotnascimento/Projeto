using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocketApp.Hubs;
using System.Text;

namespace SocketApp
{
    public class RabbitMQConsumer 
    {
        private StockHub stockHub;
        private IHubContext<StockHub> context;

        public RabbitMQConsumer(StockHub stockHub, IHubContext<StockHub> context)
        {
            this.stockHub = stockHub;
            this.context = context;
        }
        public delegate void DoAction(string data);

        public void ReceiveMessage(string queueName)
        {

            var factoryx = new ConnectionFactory() { HostName = "localhost" };
            var connection = factoryx.CreateConnection();
            var channel = connection.CreateModel();
            
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    stockHub.BotSending(message);
                    Console.WriteLine(" [x] Receive {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            
        }
    }
}
