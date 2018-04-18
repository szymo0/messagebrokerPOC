using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

namespace MessageBroker.POC.Reciver
{
    class Program
    {
        static void Main(string[] args)
        {

            Task.Run(() =>
            {
                var configRabbitMq = new EndpointConfiguration("MessageBroker.RabbitMq.Transport.0005NPIK");
                var transportRabbitMq = configRabbitMq.UseTransport<RabbitMQTransport>();
                transportRabbitMq.ConnectionString(
                    "host=hornet.rmq.cloudamqp.com;username=sojcrrfr;password=y_y7PKA38r3U3R9ZWyv90O4YziHgztCA;UseTLS=true;virtualHost=sojcrrfr");
                transportRabbitMq.UseDirectRoutingTopology();
                configRabbitMq.UsePersistence<InMemoryPersistence>();
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
                //configRabbitMq.DisableFeature<TimeoutManager>();
                configRabbitMq.EnableInstallers();

                var result = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
            });
            Task.Run(() =>
            {
                var configRabbitMq = new EndpointConfiguration("MessageBroker.RabbitMq.Transport.0002NPIK");
                var transportRabbitMq = configRabbitMq.UseTransport<RabbitMQTransport>();
                transportRabbitMq.ConnectionString(
                    "host=hornet.rmq.cloudamqp.com;username=sojcrrfr;password=y_y7PKA38r3U3R9ZWyv90O4YziHgztCA;UseTLS=true;virtualHost=sojcrrfr");
                transportRabbitMq.UseDirectRoutingTopology();
                configRabbitMq.UsePersistence<InMemoryPersistence>();
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
                //configRabbitMq.DisableFeature<TimeoutManager>();
                configRabbitMq.EnableInstallers();

                var result = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
            });

            Task.Run(() =>
            {
                var configRabbitMq = new EndpointConfiguration("MessageBroker.RabbitMq.Transport.0004NPIK");
                var transportRabbitMq = configRabbitMq.UseTransport<RabbitMQTransport>();
                transportRabbitMq.ConnectionString(
                    "host=hornet.rmq.cloudamqp.com;username=sojcrrfr;password=y_y7PKA38r3U3R9ZWyv90O4YziHgztCA;UseTLS=true;virtualHost=sojcrrfr");
                transportRabbitMq.UseDirectRoutingTopology();
                configRabbitMq.UsePersistence<InMemoryPersistence>();
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
                //configRabbitMq.DisableFeature<TimeoutManager>();
                configRabbitMq.EnableInstallers();

                var result = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
            });

            Console.ReadKey();
        }
    }
}
