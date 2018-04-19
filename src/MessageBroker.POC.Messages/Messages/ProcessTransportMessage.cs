using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class ProcessTransportMessage:IMessage
    {
        public string Data { get; set; }
        public Guid CorrelationId { get; set; }
        public string Destination { get; set; }
        public string InsertMethodName { get; set; }
        public string Soruce { get; set; }
        public string BussinesId { get; set; }
    }
}
