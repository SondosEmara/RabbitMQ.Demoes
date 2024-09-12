using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Consumer
{
    internal class Program
    {
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

            #region Exchange-Config
                //If the i run the conumder before the producer --> make error because not exist the queue....
                //If the queue exist no make again...
                var exchangeName = "Demo-Exchange-Name";
                var queueName = "Demo-Queue-Hello";
                var routingKey = "Demo-routing-key-Hello";

                //Declare the Excahnge...!
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                channel.QueueDeclare(queueName, false, false, false, null);
                channel.QueueBind(queueName, exchangeName, routingKey, null);
            #endregion

            #region Consumer-Configs
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, arguemnt) =>
                {
                    var body = arguemnt.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [Consumer] Received {message}");
                };
                channel.BasicConsume("Demo-Queue-Hello", true, consumer); 
            #endregion
        }
    }
}
