using SensorReceiver.Api.Controllers;
using SensorReceiver.Domain.Command;
using SensorReceiver.Infrastructure.Queue;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace SensorReceiver.Api.Test.Api
{
    public class EventController_Test
    {
        Mock<IQueueClient> mockQueueClient;

        public EventController_Test()
        {
            mockQueueClient = new Mock<IQueueClient>();
        }

        [Theory]
        [InlineData(1541812759, "brasil.sudeste.sensor01", "")]
        [InlineData(1541815123, "brasil.norte.sensor01", "")]
        [InlineData(1541512758, "brasil.nordeste.sensor01", "")]
        public void TestPost(long timestamp, string sensorId, string temperature)
        {
            mockQueueClient.Setup(service => service.Publish<NewEventCommand>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NewEventCommand>())).Verifiable();

            EventController controller = new EventController();
            NewEventCommand eventCommand = new NewEventCommand() { TimeStamp = timestamp, SensorId = sensorId, Temperature = temperature };
            ActionResult<NewEventCommand> result = controller.Post(eventCommand, mockQueueClient.Object);
            mockQueueClient.VerifyAll();
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(eventCommand, result.Value);
        }
    }
}
