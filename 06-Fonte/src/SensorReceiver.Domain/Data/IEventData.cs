using SensorReceiver.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SensorReceiver.Domain.Data
{
    public interface IEventData
    {
        Task Insert(Event oEvent);
    }
}
