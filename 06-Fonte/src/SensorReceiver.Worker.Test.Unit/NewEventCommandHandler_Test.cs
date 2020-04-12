using SensorReceiver.Domain.Data;
using SensorReceiver.Domain.Entities;
using SensorReceiver.Domain.Handler;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace SensorReceiver.Worker.Test.Unit
{
    public class NewEventCommandHandler_Test
    {
        Mock<IEventData> mockEventData;


        public NewEventCommandHandler_Test()
        {
            mockEventData = new Mock<IEventData>();
        }

        [Theory]
        [InlineData(1541812759, "brasil.sudeste.sensor01", "34")]
        [InlineData(1541815123, "brasil.norte.sensor01", "67")]
        [InlineData(1541512758, "brasil.nordeste.sensor01", "12")]
        public async Task InsertWithError(long timestamp, string tag, string value)
        {
            //mockEventData.Setup(service => service.Insert(It.Is<Event>(evt => evt.Error == true))).Returns(Task.CompletedTask);
            //NewEventCommandHandler handler = new NewEventCommandHandler(mockEventData.Object);
            //await handler.Handle(new Domain.Command.NewEventCommand() { TimeStamp = timestamp, Tag = tag, Value = value });
            //mockEventData.VerifyAll();
        }

        [Theory]
        [InlineData(1541812759, "brasil.sudeste.sensor01", "18392")]
        [InlineData(1541815123, "brasil.norte.sensor01", "funcionou")]
        [InlineData(1541512758, "brasil.nordeste.sensor01", "23812390")]
        public async Task InsertSuccess(long timestamp, string tag, string value)
        {
            //mockEventData.Setup(service => service.Insert(It.Is<Event>(evt => evt.Error == false))).Returns(Task.CompletedTask);
            //NewEventCommandHandler handler = new NewEventCommandHandler(mockEventData.Object);
            //await handler.Handle(new Domain.Command.NewEventCommand() { TimeStamp = timestamp, Tag = tag, Value = value });
            //mockEventData.VerifyAll();
        }
    }
}
