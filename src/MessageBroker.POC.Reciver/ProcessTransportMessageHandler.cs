using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;

namespace MessageBroker.POC.Reciver
{
    public class ProcessTransportMessageHandler: IHandleMessages<ProcessTransportMessage>
    {
        public Task Handle(ProcessTransportMessage message, IMessageHandlerContext context)
        {
            SendOptions sendOptions=new SendOptions();
            sendOptions.SetDestination("MessageBroker.RabbitMq.Transport");
            return context.Send(new TransportMessageCompleted(), sendOptions);
        }
    }
}
