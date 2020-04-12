using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using App.Metrics.AspNetCore;
using System;
using App.Metrics;

namespace SensorReceiver.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.ConfigureMetricsWithDefaults(
            //    builder =>
            //    {
            //        builder.Report.ToInfluxDb(options =>
            //        {
            //            options.InfluxDb.BaseUri = new System.Uri("http://influxdb:8086");
            //            options.InfluxDb.Database = "sensor-events";
            //            options.InfluxDb.CreateDataBaseIfNotExists = true;
            //            options.InfluxDb.UserName = "influxUser";
            //            options.InfluxDb.Password = "influxPwd";
            //        });                 
            //    })
            //.UseMetrics()
            .UseStartup<Startup>();            
    }
}
