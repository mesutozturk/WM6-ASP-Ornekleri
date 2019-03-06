using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rabbit.BLL.Repository;
using RabbitMQ.Client.Events;

namespace Rabbit.Consumer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            logCustomer = new CustomerRepo().Queryable().Count();
            logMailLog = new MailLogRepo().Queryable().Count();
        }
        public static Consumer _consumer;
        public static long logCustomer = 0, logMailLog = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            _consumer = new Consumer("Customer");
            _consumer.Form = this;
            _consumer.ConsumerEvent.Received += ConsumerEvent_Received;
            _consumer= new Consumer("MailLog");
            _consumer.ConsumerEvent.Received += ConsumerEvent_Received;
            _consumer.Form = this;
            ConsumerEvent_Received(sender, new BasicDeliverEventArgs());
        }
        private void ConsumerEvent_Received(object sender, RabbitMQ.Client.Events.BasicDeliverEventArgs e)
        {
            
        }
    }
}
