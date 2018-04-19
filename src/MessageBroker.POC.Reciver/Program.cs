using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.MessageMutator;
using NServiceBus.Persistence.Sql;

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
                var sqlPersistance = configRabbitMq.UsePersistence<SqlPersistence>();
                sqlPersistance.SqlDialect<SqlDialect.MsSqlServer>();
                sqlPersistance.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
                sqlPersistance.ConnectionBuilder(()=> new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
                sqlPersistance.TablePrefix("0005NPIK");
                configRabbitMq.RegisterMessageMutator(new IncomingHeadersMutator());
                configRabbitMq.RegisterMessageMutator(new OutComingHeadersMutator());
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
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

                var sqlPersistance = configRabbitMq.UsePersistence<SqlPersistence>();
                sqlPersistance.SqlDialect<SqlDialect.MsSqlServer>();
                sqlPersistance.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
                sqlPersistance.ConnectionBuilder(() => new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
                sqlPersistance.TablePrefix("0002NPIK");
                configRabbitMq.RegisterMessageMutator(new IncomingHeadersMutator());
                configRabbitMq.RegisterMessageMutator(new OutComingHeadersMutator());
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
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

                var sqlPersistance = configRabbitMq.UsePersistence<SqlPersistence>();
                sqlPersistance.SqlDialect<SqlDialect.MsSqlServer>();
                sqlPersistance.SubscriptionSettings().CacheFor(TimeSpan.FromDays(1));
                sqlPersistance.ConnectionBuilder(() => new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
                sqlPersistance.TablePrefix("0004NPIK");
                configRabbitMq.RegisterMessageMutator(new IncomingHeadersMutator());
                configRabbitMq.RegisterMessageMutator(new OutComingHeadersMutator());
                configRabbitMq.AuditProcessedMessagesTo("audit");
                configRabbitMq.SendFailedMessagesTo("erorrs");
                configRabbitMq.EnableDurableMessages();
                configRabbitMq.EnableInstallers();

                var result = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
            });

            Console.ReadKey();
        }
    }
}
