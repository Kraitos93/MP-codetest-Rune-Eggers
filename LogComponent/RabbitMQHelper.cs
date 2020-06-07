using RabbitMQ.Client;
using System.Text;

namespace LogTest
{
    public class RabbitMQHelper
    {
        private static ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = "localhost" };

        public static void SendMessage(string message, string routingKey = "")
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", routingKey, false, null, messageBytes);

                }
            }
        }

        public static void CreateQueue(string name)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: name,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                }
            }
        }

        public static void FlushQueue(string queuename)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueuePurge(queuename);
                }
            }
        }

        public static void DeleteQueue(string queuename)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDelete(queuename, false, false);
                }
            }
        }

        public static int GetMessageCount(string queue)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    return (int) channel.MessageCount(queue);
                }
            }
        }
    }
}
