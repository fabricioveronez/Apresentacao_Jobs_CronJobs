using App.Metrics;
using SensorReceiver.Infrastructure.Queue;
using SensorReceiver.Infrastructure.Queue.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SensorReceiver.Infrastructure.Common;

namespace SensorReceiver.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((ser) => new EnviromentConfiguration(Configuration));

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

            services.AddTransient<IQueueClient>((ser) =>
            {
                var envConf = ser.GetService<EnviromentConfiguration>();
                return new RabbitMQQueueClient(ser.GetService<IConnectionFactory>(), envConf.GetValue("QueueConfig:queue"));
            });

            IMetricsRoot metrics = AppMetrics.CreateDefaultBuilder().Build();

            //services.AddMetrics(metrics);
            //services.AddMetricsTrackingMiddleware();
            //services.AddMetricsReportScheduler();

            //services.AddMvc().AddMetrics();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMetricsAllMiddleware();

            app.UseMvc();
        }
    }
}
