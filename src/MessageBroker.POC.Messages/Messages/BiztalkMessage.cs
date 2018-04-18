using System;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class BiztalkMessage : IMessage
    {
        public Guid CorrelationId { get; set; }
        public decimal? RowId { get; set; }
        public DateTime GenerateTime { get; set; }
        public string Destination { get; set; }
        public string Data { get; set; }
        public string TranposrtName { get; set; }
    }
}