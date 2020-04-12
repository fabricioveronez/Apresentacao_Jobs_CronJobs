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

namespace SensorReceiver.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesProvider = ConfigureContainer();
            var runner = servicesProvider.GetRequiredService<App>();
            runner.Execute(args);

            Console.Read();
        }


        public static IConfigurationRoot Configuration { get; set; }

        private static ServiceProvider ConfigureContainer()
        {
            ServiceCollection services = new ServiceCollection();

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

            services.AddSingleton<IConnectionFactory>(new ConnectionFactory()
            {
                HostName = Configuration.GetSection("QueueConfig:hostName").Value,
                UserName = Configuration.GetSection("QueueConfig:userName").Value,
                Password = Configuration.GetSection("QueueConfig:password").Value
            });

            services.AddTransient((ser) =>
            {
                var db = new MongoClient($"mongodb://{Configuration.GetSection("MongoClient:User").Value}:{Configuration.GetSection("MongoClient:Password").Value}@{Configuration.GetSection("MongoClient:Host").Value}:27017/{Configuration.GetSection("MongoClient:Database").Value}");
                return db.GetDatabase(Configuration.GetSection("MongoClient:Database").Value).GetCollection<Event>("Event");
            });
            services.AddTransient<IEventData, EventData>();
            services.AddTransient<IHandler<NewEventCommand>, NewEventCommandHandler>();

            // Application
            services.AddTransient<App>();

            services.AddTransient<IQueueClient>((ser) => new RabbitMQQueueClient(ser.GetService<IConnectionFactory>(), Configuration.GetSection("QueueConfig:queue").Value));
            services.AddScoped(typeof(IQueueReceiver<>), typeof(RabbitMQQueueReceiver<>));

            return services.BuildServiceProvider();
        }
    }
}