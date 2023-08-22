using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockBotFunction;
using System.IO;
using System;

namespace StockBot
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                 .ConfigureServices(services =>
                 {
                     services.AddSingleton<IConfiguration>(data =>
                     {
                         var result = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", false, true)
                             .AddEnvironmentVariables()
                             .Build();
                         return result;
                     });

                     services.AddSingleton<IServiceProvider, ServiceProvider>();
                     services.AddTransient<IMessageBroker, RabbitMQFacade>();
                 })
                .Build();
            host.Run();
        }
    }
}