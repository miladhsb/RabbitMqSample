using RabbitMQ.Client;

namespace RabbitMqSample
{
    public class StartupConsumer:BackgroundService
    {
        private readonly IRabbitManager _rabbitManager;
        public StartupConsumer(IRabbitManager rabbitManager)
        {
            _rabbitManager = rabbitManager;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken )
        {
            _rabbitManager.Createconsumer(QueueName: "milad",
                exchangeName: "milad.person",
                exchangeType: ExchangeType.Topic,
                routeKey: "*.queue.durable.dotnetcore.#");

            return Task.CompletedTask;
        }
    }
}
