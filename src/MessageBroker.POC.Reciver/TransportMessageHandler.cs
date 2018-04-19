using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;

namespace MessageBroker.POC.Reciver
{
    public class TransportMessageHandler : IHandleMessages<TransportMessage>
    {
        public Task Handle(TransportMessage message, IMessageHandlerContext context)
        {
            Console.WriteLine(message.Destination + " " + message.Data);
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination(context.ReplyToAddress);
            var task = context.Send(new TransportMessageRecived
            {
                BussinesId = message.BussinesId
            }, sendOptions);
            task.Wait();
            return context.SendLocal(new ProcessTransportMessage
            {
                BussinesId = message.BussinesId,
                CorrelationId = message.CorrelationId,
                Data = message.Data,
                InsertMethodName = message.InsertMethodName,
                Destination = message.Destination,
                Soruce = message.Soruce
            });
        }
    }
}
