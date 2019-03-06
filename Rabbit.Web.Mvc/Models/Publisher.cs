using Rabbit.BLL.RabbitMq;
using RabbitMQ.Client;
using System.Text;

namespace Rabbit.Web.Mvc.Models
{
    public class Publisher
    {
        private readonly RabbitMqService _rabbitMqService;
        private const string DefaultQueue = "wissen1";
        public Publisher(string message, string queueName = null)
        {
            if (string.IsNullOrEmpty(queueName))
                queueName = DefaultQueue;
            _rabbitMqService = new RabbitMqService();

            using (var connection = _rabbitMqService.GetRabbitMqConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    channel.BasicPublish(string.Empty, queueName, null, Encoding.UTF8.GetBytes(message));

                    //Console.WriteLine($"{queueName} queue'su üzerine, \"{message}\" mesajı yazıldı.");
                }
            }
        }
    }
}