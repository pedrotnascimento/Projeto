using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StockBotFunction;

namespace StockBot
{
    public  class StockBot
    {

        // Não conseguiu fazer injeção, vou usar implementação
        //private IMessageBroker messageBroker;
        //public StockBot(IMessageBroker messageBroker)
        //{
        //    this.messageBroker = messageBroker;
        //}

        [Function("stockbot")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get" , Route = "stockbot/{stock}")] HttpRequestData req, string stock,
            FunctionContext executionContext, IConfiguration configuration)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");
            double? closePrice = await StockManager.GetClosePriceOfStock(stock);

            string messageToBroker = $"APPL.US quote is ${closePrice} per share";
            logger.LogInformation(messageToBroker);

            var queueName = GetEnvironmentVariable("QueueName");
            var messageBroker = new RabbitMQFacade(queueName);
            messageBroker.SendMessage(messageToBroker);
            //messageBroker.ReceiveMessage();
            //Console.ReadLine();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
