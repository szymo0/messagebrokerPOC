using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.InMemory.Outbox;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
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
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.RegisterMessageMutator(new IncomingHeadersMutator());
            endpointConfiguration.RegisterMessageMutator(new OutComingHeadersMutator());

            var sqlPersistance = endpointConfiguration.UsePersistence<SqlPersistence>();
            sqlPersistance.SqlDialect<SqlDialect.MsSqlServer>();
            sqlPersistance.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
            var connString = ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString;
            sqlPersistance.ConnectionBuilder(() => new SqlConnection(connString));
            sqlPersistance.TablePrefix("Sender");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.UseDeadLetterQueueForMessagesWithTimeToBeReceived();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.EnableJournaling();
            endpointConfiguration.EnableInstallers();


            var configRabbitMq=new EndpointConfiguration("MessageBroker.RabbitMq.Transport");
            configRabbitMq.Recoverability().Delayed(c => c.NumberOfRetries(1).TimeIncrease(TimeSpan.FromMinutes(5)));
            configRabbitMq.EnableDurableMessages();
            configRabbitMq.EnableOutbox().TimeToKeepDeduplicationData(TimeSpan.FromDays(1));
            configRabbitMq.RegisterMessageMutator(new IncomingHeadersMutator());
            configRabbitMq.RegisterMessageMutator(new OutComingHeadersMutator());
            var transportRabbitMq = configRabbitMq.UseTransport<RabbitMQTransport>();
            transportRabbitMq.ConnectionString("host=hornet.rmq.cloudamqp.com;username=sojcrrfr;password=y_y7PKA38r3U3R9ZWyv90O4YziHgztCA;UseTLS=true;virtualHost=sojcrrfr");
            transportRabbitMq.UseDirectRoutingTopology();
            transportRabbitMq.UseDurableExchangesAndQueues(true);

            var sqlPersistance2 = configRabbitMq.UsePersistence<SqlPersistence>();
            sqlPersistance2.SqlDialect<SqlDialect.MsSqlServer>();
            sqlPersistance2.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
            sqlPersistance2.ConnectionBuilder(() => new SqlConnection(connString));
            sqlPersistance2.TablePrefix("Sender_rabbit");

            configRabbitMq.AuditProcessedMessagesTo("audit");
            configRabbitMq.SendFailedMessagesTo("erorrs");
            configRabbitMq.EnableDurableMessages();
            //configRabbitMq.DisableFeature<TimeoutManager>();
            configRabbitMq.EnableInstallers();

            var endpoint2 = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            Console.Write("Ready to go");
            Console.ReadLine();
        }
    }
}
