using App.Application.Contracts.BusServices;
using App.Bus.Consumers;
using App.Domain.Consts;
using App.Domain.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Bus
{
    public static class BusExtensions
    {
        public static void AddBusExt(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusOption = configuration.GetSection(nameof(ServiceBusOption)).Get<ServiceBusOption>();

            services.AddScoped<IBusService, BusService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductAddedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(serviceBusOption.Url);
                    cfg.ReceiveEndpoint(BusServiceConst.ProductAddedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<ProductAddedEventConsumer>(context);
                    });
                });
            });
        }
    }
}
