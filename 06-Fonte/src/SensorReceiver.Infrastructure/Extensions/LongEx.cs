using System;
using System.Collections.Generic;
using System.Text;

namespace SensorReceiver.Infrastructure.Extensions
{
    public static class LongEx
    {
        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }      
    }
}
