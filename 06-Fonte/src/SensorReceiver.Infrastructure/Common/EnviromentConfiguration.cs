using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorReceiver.Infrastructure.Common
{
    public class EnviromentConfiguration
    {
        private readonly IConfiguration _configuration;

        public EnviromentConfiguration(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GetValue(string key)
        {
            string value = Environment.GetEnvironmentVariable(key.Replace(":", "_").ToUpper());

            if (string.IsNullOrEmpty(value))
            {
                value = _configuration[key];
            }

            return value;
        }
    }
}
