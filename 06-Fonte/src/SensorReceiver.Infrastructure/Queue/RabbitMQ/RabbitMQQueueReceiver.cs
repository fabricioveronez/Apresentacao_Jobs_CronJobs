using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SensorReceiver.Infrastructure.Queue.RabbitMQ
{
    public class RabbitMQQueueReceiver<T> : IQueueReceiver<T>
    {
        private IQueueClient _client;
        private IModel model;
        private IConnection con;

        public RabbitMQQueueReceiver(IQueueClient client)
        {
            this._client = client;
        }

        Func<T, Task> _action;

        public void OnReceiver(Func<T, Task> action)
        {
            this._action = action;
        }

        public void Start()
        {
            var retryOnStartupPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(9, retryAttempt =>
                     TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                 );

            retryOnStartupPolicy.Execute(() =>
            {
                con = this._client.GetConnection();
                model = this._client.GetModel(con);               

                EventingBasicConsumer consumer = new EventingBasicConsumer(model);
                consumer.Received += async (m, ea) =>
                {
                    T oEvent = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ea.Body));
                    try
                    {
                        await this._action(oEvent);
                        model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception e)
                    {
                        model.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                model.BasicConsume(queue: this._client.GetQueueName(),
                                        autoAck: false,
                                        consumer: consumer);
            });
        }
    }
}
