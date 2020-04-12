using Newtonsoft.Json;

namespace SensorReceiver.Domain.Command
{
    public class NewEventCommand
    {
        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        [JsonProperty("sensorId")]
        public string SensorId { get; set; }

        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }
}
