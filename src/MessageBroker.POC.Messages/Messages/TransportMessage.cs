using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class TransportMessage : IMessage
    {
        public string Data { get; set; }
        public DateTime ForwardDate { get; set; }
        public Guid ReletedMessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public decimal? RowId { get; set; }
        public string TranposrtName { get; set; }
        public string Destination { get; set; }
    }
}
