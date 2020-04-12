using System;

namespace SensorReceiver.Domain.Entities
{
    public class Event
    {
        public long TimeStamp { get; set; }
        public string SensorId { get; set; }
        public string Temperature { get; set; }
    }
}
