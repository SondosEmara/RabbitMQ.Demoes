using RabbitMQ.Client;

namespace RabbitMQ.Producer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // Connect to the local RabbitMQ server
                Port = 5672,            // Default AMQP port
                UserName = "guest",     // Default username
                Password = "guest"      // Default password
            };

            using (var connection = factory.CreateConnection())
            {
                Console.WriteLine("Connected to RabbitMQ server locally!");
            }
        }
    }
}
