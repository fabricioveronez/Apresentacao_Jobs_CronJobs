using App.Metrics;
using SensorReceiver.Infrastructure.Queue;
using SensorReceiver.Infrastructure.Queue.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

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
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory()
            {
                HostName = Configuration.GetSection("QueueConfig:hostName").Value,
                UserName = Configuration.GetSection("QueueConfig:userName").Value,
                Password = Configuration.GetSection("QueueConfig:password").Value
            });

            services.AddTransient<IQueueClient>((ser) => new RabbitMQQueueClient(ser.GetService<IConnectionFactory>(), Configuration.GetSection("QueueConfig:queue").Value));

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
