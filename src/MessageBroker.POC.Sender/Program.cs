using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

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

            var sqlPersistance = endpointConfiguration.UsePersistence<SqlPersistence>();
            sqlPersistance.SqlDialect<SqlDialect.MsSqlServer>();
            sqlPersistance.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
            sqlPersistance.ConnectionBuilder(() => new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
            sqlPersistance.TablePrefix("Sender");

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

            var sqlPersistance2 = endpointConfiguration.UsePersistence<SqlPersistence>();
            sqlPersistance2.SqlDialect<SqlDialect.MsSqlServer>();
            sqlPersistance2.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
            sqlPersistance2.ConnectionBuilder(() => new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
            sqlPersistance2.TablePrefix("Sender_rabbit");

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
