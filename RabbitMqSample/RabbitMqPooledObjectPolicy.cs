using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace RabbitMqSample
{



    public class RabbitMqPooledObjectPolicy: IPooledObjectPolicy<IModel>
    {
  
            private readonly RabbitOptions _options;

            private readonly IConnection _connection;

            public RabbitMqPooledObjectPolicy(IOptions<RabbitOptions> RabbitOption)  
            {
                 _options = RabbitOption.Value;

                _connection = GetConnection();
            }

            private IConnection GetConnection()
            {
                var factory = new ConnectionFactory()
                {
                   
                    HostName = _options.HostName,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    Port = _options.Port,
                    VirtualHost= _options.VirtualHost
                  
                };

                return factory.CreateConnection();
            }

            public IModel Create()
            {
                var channel = _connection.CreateModel();
          
                channel.ConfirmSelect();
                return channel;
            }

            public bool Return(IModel obj)
            {

            // Console.WriteLine("Rabbit Queue connection is open " + isopen);

            // logger.Log(LogLevel.Information, "Rabbit Queue connection is open " + isopen);

            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
          
        }

        }
    }

