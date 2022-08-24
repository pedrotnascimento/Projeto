namespace SocketApp
{
    public static class ApplicationBuilderExtentions
    {
        public static RabbitMQConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitMQConsumer>();

            var life = app.ApplicationServices.GetService<Microsoft.AspNetCore.Hosting.IApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            //life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            //var name = Environment.GetEnvironmentVariable("QueueName", EnvironmentVariableTarget.Process);
            Listener.ReceiveMessage("StockBotQueue");
        }
    }
}
