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

            return app;
        }

        private static void OnStarted()
        {
            Listener.ReceiveMessage("StockBotQueue");
        }
    }
}
