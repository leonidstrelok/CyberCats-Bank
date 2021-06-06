using System;
using ActivityLogger.EventBus;
using CyberCats_Bank.EventBus.Contracts.Logging;
using CyberCats_Bank.EventBus.Options;
using MassTransit;
using MassTransit.MessageData;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CyberCats_Bank.EventBus
{
    public static class DependencyInjection
    {
        public static void AddEventBus(this IServiceCollection services, IHostEnvironment environment,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqOption>(p =>
            {
                var obj = new
                {
                    Password = configuration.GetSection("EventBus:Password").Value,
                    PortNumber = Convert.ToUInt16(configuration.GetSection("EventBus:PortNumber").Value),
                    IPAddress = configuration.GetSection("EventBus:IPAddress").Value,
                    UserName = configuration.GetSection("EventBus:UserName").Value
                };
                p.Password = obj.Password;
                p.PortNumber = obj.PortNumber;
                p.IPAddress = obj.IPAddress;
                p.UserName = obj.UserName;
            });
            IMessageDataRepository messageDataRepository = CreateMessageDataRepository(environment, configuration);

            services.AddMassTransit(options =>
            {
                options.AddConsumer<LoggingConsumer>();
                options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseMessageData(messageDataRepository);
                    var options = provider.GetRequiredService<IOptions<RabbitMqOption>>().Value;
                    EndpointConvention.Map<LogContract>(new Uri("queue:logging"));
                    
                    cfg.Host(new Uri(options.IPAddress), "/", h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });
                    
                }));
            });
            services.AddMassTransitHostedService();
        }

        private static IMessageDataRepository CreateMessageDataRepository(IHostEnvironment environment,
            IConfiguration configuration)
        {
            if (environment.IsDevelopment())
                return new InMemoryMessageDataRepository();
            return new MongoDbMessageDataRepository(configuration["Mongo:ConnectionString"],
                configuration["Mongo:MassTransitDatabase"]);
        }
    }
}