using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMqSample
{
    public interface IRabbitManager
    {
        void Createconsumer(string QueueName, string exchangeName, string exchangeType, string routeKey);
        void Publish<T>(T message, string QueueName , string exchangeName, string exchangeType, string routeKey)
        where T : class;
    }


    public class RabbitManager : IRabbitManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }


        public void Publish<T>(T message,string QueueName, string exchangeName, string exchangeType, string routeKey)
            where T : class
        {
            if (message == null)
                return;
            var channel = _objectPool.Get();
            try
            {
              
                channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true, autoDelete: false, arguments: null);
                //var MyQueue = channel.QueueDeclare(queue: QueueName, durable: true, exclusive: true, autoDelete: false, arguments: null);
                var MyQueue = channel.QueueDeclarePassive(QueueName);
                
                channel.QueueBind(queue: MyQueue.QueueName, exchange: exchangeName, routingKey: routeKey, arguments: null);

                var sendBytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));
                var properties = channel.CreateBasicProperties();
               
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }


        public void Createconsumer(string QueueName, string exchangeName, string exchangeType, string routeKey)
        {
            var channel = _objectPool.Get();

       


            try
            {
                //var queueArgs = new Dictionary<string, object> {

                // { "x-message-ttl", 2000 } };
                channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true, autoDelete: false,arguments: null);
                var MyQueue = channel.QueueDeclare(queue: QueueName,durable: true,exclusive: false,autoDelete: false,arguments: null);
                channel.QueueBind(queue: MyQueue.QueueName,exchange: exchangeName,routingKey: routeKey,arguments: null);


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                   // channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: QueueName,
                                     autoAck: false,
                                     consumer: consumer);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }


        }

    }
}
