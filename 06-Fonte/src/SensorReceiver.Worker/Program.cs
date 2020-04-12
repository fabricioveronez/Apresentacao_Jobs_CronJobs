using SensorReceiver.Infrastructure.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.IO;
using SensorReceiver.Infrastructure.Queue.RabbitMQ;
using SensorReceiver.Domain.Data;
using SensorReceiver.Infrastructure.Handler;
using SensorReceiver.Domain.Command;
using SensorReceiver.Domain.Handler;
using MongoDB.Driver;
using SensorReceiver.Domain.Entities;
using SensorReceiver.Infrastructure.Common;
using System.Threading;

namespace SensorReceiver.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(60000);
            var servicesProvider = ConfigureContainer();
            var runner = servicesProvider.GetRequiredService<App>();
            runner.Execute(args);

            Console.Read();
        }


        public static IConfigurationRoot Configuration { get; set; }

        private static ServiceProvider ConfigureContainer()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton((ser) => new EnviromentConfiguration(Configuration));

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

            services.AddSingleton<IConnectionFactory>((ser) =>
            {
                var envConf = ser.GetService<EnviromentConfiguration>();
                return new ConnectionFactory()
                {
                    HostName = envConf.GetValue("QueueConfig:hostName"),
                    UserName = envConf.GetValue("QueueConfig:userName"),
                    Password = envConf.GetValue("QueueConfig:password")
                };
            });

            services.AddTransient((ser) =>
            {
                var envConf = ser.GetService<EnviromentConfiguration>();
                var db = new MongoClient($"mongodb://{envConf.GetValue("MongoClient:User")}:{envConf.GetValue("MongoClient:Password")}@{envConf.GetValue("MongoClient:Host")}:27017/{envConf.GetValue("MongoClient:Database")}");
                return db.GetDatabase(envConf.GetValue("MongoClient:Database")).GetCollection<Event>("Event");
            });
            services.AddTransient<IEventData, EventData>();
            services.AddTransient<IHandler<NewEventCommand>, NewEventCommandHandler>();

            // Application
            services.AddTransient<App>();

            services.AddTransient<IQueueClient>((ser) =>
            {
                var envConf = ser.GetService<EnviromentConfiguration>();
                return new RabbitMQQueueClient(ser.GetService<IConnectionFactory>(), envConf.GetValue("QueueConfig:queue"));
            });
            services.AddScoped(typeof(IQueueReceiver<>), typeof(RabbitMQQueueReceiver<>));

            return services.BuildServiceProvider();
        }
    }
}