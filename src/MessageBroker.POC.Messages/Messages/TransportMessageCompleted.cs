using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class TransportMessageCompleted : IMessage
    {
        public string BussinesId { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
