using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;

namespace MessageBroker.POC.Reciver
{
    public class TransportMessageHandler:IHandleMessages<TransportMessage>
    {
        public Task Handle(TransportMessage message, IMessageHandlerContext context)
        {
            Console.WriteLine(message.Destination + " " + message.Data);
            return Task.CompletedTask;
        }
    }
}
