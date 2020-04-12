using SensorReceiver.Domain.Command;
using SensorReceiver.Infrastructure.Handler;
using SensorReceiver.Infrastructure.Queue;
using SensorReceiver.Infrastructure.Queue.RabbitMQ;

namespace SensorReceiver.Worker
{
    public class App
    {

        private IQueueReceiver<NewEventCommand> _receiver;
        private IHandler<NewEventCommand> _eventHandler;

        public App(IHandler<NewEventCommand> eventHandler, IQueueReceiver<NewEventCommand> receiver)
        {
            this._receiver = receiver;
            this._eventHandler = eventHandler;
        }

        public void Execute(string[] args)
        {
            this._receiver.OnReceiver(async (e) => await this._eventHandler.Handle(e));
            this._receiver.Start();
        }
    }
}
