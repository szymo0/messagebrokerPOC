using System;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class TransportMessageRecived : IMessage
    {
        public string BussinesId { get; set; }
        public Guid CorrelationId { get; set; }
    }
}