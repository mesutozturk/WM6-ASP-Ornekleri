using Newtonsoft.Json;
using Rabbit.BLL.RabbitMq;
using Rabbit.Models.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Consumer
{
    public class Consumer
    {
        private readonly RabbitMqService _rabbitMqService;
        public EventingBasicConsumer ConsumerEvent;
        public Consumer(string queueName)
        {
            _rabbitMqService = new RabbitMqService();
            var connection = _rabbitMqService.GetRabbitMqConnection();
            var channel = connection.CreateModel();
            ConsumerEvent = new EventingBasicConsumer(channel);
            // Received event'i sürekli listen modunda olacaktır.
            ConsumerEvent.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                //var data = $"{queueName} isimli queue üzerinden gelen mesaj: \"{message}\"";
                if (queueName == "MailLog")
                {
                    var data = JsonConvert.DeserializeObject<List<MailLog>>(message);
                    //işlemler
                }
                else if (queueName == "Customer")
                {
                    var data = JsonConvert.DeserializeObject<List<Customer>>(message);
                    //işlemler
                }
            };
            channel.BasicConsume(queueName, true, ConsumerEvent);
        }
    }
}
