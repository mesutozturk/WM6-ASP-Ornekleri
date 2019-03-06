using Newtonsoft.Json;
using Rabbit.BLL.RabbitMq;
using Rabbit.Models.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.VisualStyles;
using Rabbit.BLL.Repository;

namespace Rabbit.Consumer
{
    public class Consumer
    {
        private readonly RabbitMqService _rabbitMqService;
        public EventingBasicConsumer ConsumerEvent;
        public Form1 Form { get; set; }
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
                    var data = JsonConvert.DeserializeObject<MailLog>(message);
                    //işlemler
                    new MailLogRepo().Insert(new MailLog()
                    {
                        Id = data.Id,
                        CustomerId = data.CustomerId,
                        Message = data.Message,
                        Subject = data.Subject
                    });
                    Form1.logMailLog++;
                    Form.Text = $"Customer {Form1.logCustomer} - MailLog {Form1.logMailLog}";
                }
                else if (queueName == "Customer")
                {
                    var data = JsonConvert.DeserializeObject<List<Customer>>(message);
                    var repo = new CustomerRepo();
                    for (var i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        Form1.logCustomer++;
                        Form.Text = $"Customer {Form1.logCustomer} - MailLog {Form1.logMailLog}";
                        repo.InsertForMark(new Customer()
                        {
                            Address = item.Address,
                            Email = item.Email,
                            Id = item.Id,
                            Name = item.Name,
                            Phone = item.Phone,
                            Surname = item.Surname,
                            RegisterDate = item.RegisterDate
                        });
                        if (i % 100 == 0)
                            repo.Save();
                    }
                    repo.Save();

                    //işlemler
                }


            };
            channel.BasicConsume(queueName, true, ConsumerEvent);
        }
    }
}
