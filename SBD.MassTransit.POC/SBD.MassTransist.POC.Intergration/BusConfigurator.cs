using MassTransit;
using MassTransit.RabbitMqTransport;
using SBD.MassTransist.POC.Intergration.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBD.MassTransist.POC.Intergration
{
    /// <summary>
    /// BusConfigurator handles MassTransit bus configuration for RabbitMQ.
    /// </summary>
    public static class BusConfigurator
    {
        /// <summary>
        /// Configures the bus based on QueueSettings
        /// </summary>
        /// <param name="queueSettings">QueueSettings objeect</param>
        /// <param name="registrationAction"></param>
        /// <returns>IBusControl</returns>
        public static IBusControl ConfigureBus(IQueueSettings queueSettings, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(queueSettings.RabbitMqUri + queueSettings.VirtualHost + "/"), hst =>
                {
                    hst.Username(queueSettings.UserName);
                    hst.Password(queueSettings.Password);
                });

                cfg.ConfigureSend(x => x.UseSendExecute(context =>
                {
                    context.Headers.Set("VirtualHost", queueSettings.VirtualHost);
                    context.Headers.Set("QueueName", queueSettings.QueueName);
                }));

                cfg.UseMessageScheduler(new Uri(queueSettings.RabbitMqUri + queueSettings.VirtualHost + "/" + queueSettings.SchedulerQueueName));

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}
