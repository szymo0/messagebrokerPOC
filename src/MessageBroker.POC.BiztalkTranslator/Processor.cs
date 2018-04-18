using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Messaging;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.InMemory.Outbox;
using NServiceBus.Persistence.Sql;

namespace MessageBroker.POC.BiztalkTranslator
{

    public class Processor
    {
        private IEndpointInstance GetSendEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("MessageBroker.Msmq.MessageToTransport");
            endpointConfiguration.SendFailedMessagesTo("MessageBroker.Msmq.MessageToTransport.Errors");
            endpointConfiguration.AuditProcessedMessagesTo("MessageBroker.Msmq.MessageToTransport.Audids");
            endpointConfiguration.Recoverability()
                .Immediate(c=>c.NumberOfRetries(5))
                .Delayed(c => c.NumberOfRetries(3).TimeIncrease(new TimeSpan(0,0,2,0)));
            endpointConfiguration.EnableOutbox().TimeToKeepDeduplicationData(TimeSpan.FromDays(1));

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(()=>new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString));
            persistence.TablePrefix("biztalk");
            persistence.SubscriptionSettings().CacheFor(TimeSpan.FromMinutes(1));

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.DisableDeadLetterQueueing();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.EnableJournaling();

            var route = transport.Routing();
            route.RouteToEndpoint(typeof(BiztalkMessage), "MessageBroker.Msmq.MessageToTransport.Published");

            endpointConfiguration.SendOnly();

            endpointConfiguration.EnableInstallers();

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            return endpoint;
        }

        public void StartProcess()
        {
            MessageQueue rQueue = new MessageQueue($@"{Environment.MachineName}\Private$\FromSql");
            if (!MessageQueue.Exists($@"{Environment.MachineName}\Private$\FromSql.error"))
                MessageQueue.Create($@"{Environment.MachineName}\Private$\FromSql.error", true);
            MessageQueue errorQueue = new MessageQueue($@"{Environment.MachineName}\Private$\FromSql.error");
            BiztalkParser biztalkParser=new BiztalkParser(new BiztalkRowParser(new MetadataParser()));

            var endpoint = GetSendEndpoint();
            rQueue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
            rQueue.MessageReadPropertyFilter.SetAll();
            rQueue.BeginReceive();
            rQueue.ReceiveCompleted += (sender, eventArgs) =>
            {
                var cmq = ((MessageQueue)sender);
                cmq.EndReceive(eventArgs.AsyncResult);
                try
                {
                    foreach (var message in biztalkParser.ParseMessage(eventArgs.Message.Body.ToString()))
                    {
                        endpoint.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    errorQueue.Send(eventArgs.Message.Body,MessageQueueTransactionType.Single);
                    Console.WriteLine(ex.Message +" " +ex.StackTrace);
                }

                cmq.Refresh();
                cmq.BeginReceive();
            };
        }

    }
}
