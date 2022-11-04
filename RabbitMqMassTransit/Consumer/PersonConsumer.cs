using MassTransit;
using RabbitMqMassTransit.DTOs;

namespace RabbitMqMassTransit.Consumer
{


    //class PersonConsumer : IConsumer<Person>
    //{
    //    public Task Consume(ConsumeContext<Person> context)
    //    {
    //        Console.WriteLine(context.Message.Name);
    //        return Task.CompletedTask;
    //    }
    //}

    class SubmitOrderConsumer :
      IConsumer<Person>
    {
        readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<Person> context)
        {
            _logger.LogInformation("Order Submitted: {OrderId}", context.Message.Name);


        }
    }

    class SubmitOrderConsumerDefinition :
        ConsumerDefinition<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerDefinition()
        {
            // override the default endpoint name
            EndpointName = "order-service";
          
            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
          
            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        
        }

    }
}