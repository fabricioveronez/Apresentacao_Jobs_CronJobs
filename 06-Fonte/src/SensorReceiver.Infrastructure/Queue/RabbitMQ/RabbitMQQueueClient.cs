using RabbitMQ.Client;
using System;
using System.Text;

namespace SensorReceiver.Infrastructure.Queue.RabbitMQ
{
    public class RabbitMQQueueClient : IQueueClient
    {

        private IConnectionFactory _connectionFactory;
        private string _queueName;

        public RabbitMQQueueClient(IConnectionFactory connectionFactory, string queueName)
        {
            this._connectionFactory = connectionFactory;
            this._queueName = queueName;
        }

        public IConnection GetConnection()
        {
            return this._connectionFactory.CreateConnection();
        }

        public IModel GetModel(IConnection connection)
        {
            IModel model = connection.CreateModel();

            model.QueueDeclare(queue: this._queueName,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

            return model;
        }

        public void Publish<T>(string exchangeName, string routingKey, T content)
        {
            string serializedContent = Newtonsoft.Json.JsonConvert.SerializeObject(content, Newtonsoft.Json.Formatting.Indented);
            using (IConnection con = this.GetConnection())
            using (IModel model = this.GetModel(con))
            {
                model.QueueDeclare(queue: this._queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                IBasicProperties properties = model.CreateBasicProperties();
                properties.Persistent = true;

                byte[] payload = Encoding.UTF8.GetBytes(serializedContent);
                model.BasicPublish(exchangeName, routingKey, properties, payload);
            }
        }

        public string GetQueueName()
        {
            return this._queueName;
        }
    }
}
