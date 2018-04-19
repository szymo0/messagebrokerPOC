using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace MessageBroker.POC.Sender.Handlers
{
    public class BiztalkMessageHandler:IHandleMessages<BiztalkMessage>
    {
        private static readonly ILog Log = LogManager.GetLogger<BiztalkMessageHandler>();
        private static IEndpointInstance _instance;
        private object _sync=new object();
        public BiztalkMessageHandler()
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        var configRabbitMq = new EndpointConfiguration("MessageBroker.RabbitMq.Transport");
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

                        _instance = Endpoint.Start(configRabbitMq).GetAwaiter().GetResult();
                    }
                }

            }
        }
        public Task Handle(BiztalkMessage message, IMessageHandlerContext context)
        {
            Log.Debug("Process");
            Log.Debug(message.Data);
            try
            {
                TransportMessage transportMessage = new TransportMessage
                {
                    CorrelationId = message.CorrelationId,
                    Data = message.Data,
                    Destination = message.Destination,
                    InsertMethodName = message.InsertMethodName,
                    BussinesId = message.BussinesId,
                    ForwardDate = DateTime.Now,
                    Soruce = message.Source

                };

                SendOptions sendOptions = new SendOptions();
                sendOptions.SetDestination($"MessageBroker.RabbitMq.Transport.{message.Destination}");
                sendOptions.SetHeader("MessageBroker.BussinesId", message.BussinesId);
                sendOptions.SetHeader("MessageBroker.Dest", message.Destination);
                sendOptions.SetHeader("MessageBroker.Src", message.Source);
                //_instance.Send(transportMessage,sendOptions).GetAwaiter().GetResult();
                _instance.SendLocal(new TransportMessageSend()
                {
                    BussinesId = message.BussinesId,
                    CorrelationId = message.CorrelationId,

                });
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }

}
