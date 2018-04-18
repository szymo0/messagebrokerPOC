﻿using System;
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
                    RowId = message.RowId,
                    ForwardDate = DateTime.Now,
                    ReletedMessageId = Guid.Parse(context.MessageHeaders["NServiceBus.MessageId"]),
                    TranposrtName = message.TranposrtName

                };

                SendOptions sendOptions = new SendOptions();
                sendOptions.SetDestination($"MessageBroker.RabbitMq.Transport.{message.Destination}");

                _instance.Send(transportMessage, sendOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
    }

}
