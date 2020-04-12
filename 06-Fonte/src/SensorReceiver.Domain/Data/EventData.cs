using MongoDB.Driver;
using SensorReceiver.Domain.Entities;
using System.Threading.Tasks;

namespace SensorReceiver.Domain.Data
{
    public class EventData : IEventData
    {
        private IMongoCollection<Event> _collection;

        public EventData(IMongoCollection<Event> collection)
        {
            this._collection = collection;
        }

        public async Task Insert(Event oEvent)
        {
            await this._collection.InsertOneAsync(oEvent);
        }
    }
}
