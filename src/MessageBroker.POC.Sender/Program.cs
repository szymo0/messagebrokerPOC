using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

namespace MessageBroker.POC.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            NServiceBus.Logging.LogManager.Use<DefaultFactory>();
            var endpointConfiguration = new EndpointConfiguration("MessageBroker.Msmq.MessageToTransport.Published");
            endpointConfiguration.SendFailedMessagesTo("MessageBroker.Msmq.MessageToTransport.Errors");
            endpointConfiguration.AuditProcessedMessagesTo("MessageBroker.Msmq.MessageToTransport.Audids");
            endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(3).TimeIncrease(new TimeSpan(0, 5, 0, 0)));
            endpointConfiguration.UsePersistence<MsmqPersistence>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.UseDeadLetterQueueForMessagesWithTimeToBeReceived();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.EnableJournaling();
            endpointConfiguration.EnableInstallers();

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var configRabbitMq=new EndpointConfiguration("MessageBroker.RabbitMq.Transport");
            var transportRabbitMq = configRabbitMq.UseTransport<RabbitMQTransport>();
            transportRabbitMq.ConnectionString("host=hornet.rmq.cloudamqp.com;username=sojcrrfr;password=y_y7PKA38r3U3R9ZWyv90O4YziHgztCA;UseTLS=true;virtualHost=sojcrrfr");
            transportRabbitMq.UseDirectRoutingTopology();
            transportRabbitMq.UseDurableExchangesAndQueues(true);
            configRabbitMq.UsePersistence<InMemoryPersistence>();
            configRabbitMq.AuditProcessedMessagesTo("audit");
            configRabbitMq.SendFailedMessagesTo("erorrs");
            configRabbitMq.EnableDurableMessages();
            //configRabbitMq.DisableFeature<TimeoutManager>();
            configRabbitMq.EnableInstallers();
            
            var endpoint2 = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();

            Console.Write("Ready to go");
            Console.ReadLine();
        }
    }
}
