using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;
using NServiceBus.Features;

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
            endpointConfiguration.UsePersistence<MsmqPersistence>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            //transport.UseDeadLetterQueueForMessagesWithTimeToBeReceived();
            transport.DisableDeadLetterQueueing();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.EnableJournaling();
            var route = transport.Routing();
            route.RouteToEndpoint(typeof(BiztalkMessage), "MessageBroker.Msmq.MessageToTransport.Published");
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
