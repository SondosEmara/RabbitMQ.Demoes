using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Producer
{
    internal class Program
    {

        /*
                    *  Queue Declare Partmaters:
                       Name
                       Durable (the queue will survive a broker restart)
                       Exclusive (used by only one connection and the queue will be deleted when that connection closes)
                       Auto-delete (queue that has had at least one consumer is deleted when last consumer unsubscribes)
                       Arguments (optional; used by plugins and broker-specific features such as message TTL, queue length limit, etc)
                       Declaring a queue is idempotent - it will only be created if it doesn't exist already. The message content is a byte array, so you can encode whatever you like there.
         */

        static void Main(string[] args)
        {
            #region Server-Establish-Connection
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

            #endregion


            #region Queue Declare && Message

                var exchangeName = "Demo-Exchange-Name";
                var queueName_1 = "Demo-Queue-Hello";
                var queueName_2 = "Demo-Queue2";
                var routingKey_1 = "Demo-routing-key-Hello";
                var routingKey_2= "Demo-routing-key2";


                //Declare the Excahnge...!
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

                channel.QueueDeclare(queueName_1, false, false, false, null);
                channel.QueueDeclare(queueName_2, false, false, false, null);
                channel.QueueBind(queueName_1, exchangeName, routingKey_1, null);
                channel.QueueBind(queueName_2,exchangeName,routingKey_2,null);

                //Message
                const string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey_1,
                                     basicProperties: null,
                                     body: body);

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey_2,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [Publisher] Sent {message}");
 
            #endregion

        }
    }
}
