using SensorReceiver.Domain.Command;
using SensorReceiver.Domain.Data;
using SensorReceiver.Domain.Entities;
using SensorReceiver.Infrastructure.Extensions;
using SensorReceiver.Infrastructure.Handler;
using System.Threading.Tasks;

namespace SensorReceiver.Domain.Handler
{
    public class NewEventCommandHandler : IHandler<NewEventCommand>
    {

        private IEventData _eventData;

        public NewEventCommandHandler(IEventData eventData)
        {
            this._eventData = eventData;
        }

        public async Task Handle(NewEventCommand command)
        {
            Event oEvent = new Event()
            {
                TimeStamp = command.TimeStamp,
                SensorId = command.SensorId,
                Temperature = command.Temperature,
            };

            await this._eventData.Insert(oEvent);
        }
    }
}
