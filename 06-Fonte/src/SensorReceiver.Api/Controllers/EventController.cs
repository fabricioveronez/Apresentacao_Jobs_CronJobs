using Microsoft.AspNetCore.Mvc;
using SensorReceiver.Domain.Command;
using SensorReceiver.Infrastructure.Queue;
using System;

namespace SensorReceiver.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpPost]
        public ActionResult<NewEventCommand> Post([FromBody] NewEventCommand newEvent, [FromServices] IQueueClient queueClient)
        {
            try
            {
                queueClient.Publish<NewEventCommand>("", "sensor-event", newEvent);
                return newEvent;
            }
            catch (Exception)
            {
                return BadRequest("Erro ao receber o evento.");
            }
        }

    }
}