using System;
using NServiceBus;

namespace MessageBroker.POC.Messages.Messages
{
    public class BiztalkMessage : IMessage
    {
        public BiztalkMessage()
        {
            Version = 1.0m;
        }

        public Guid CorrelationId { get; set; }
        public string InsertMethodName { get; set; }
        public string BussinesId { get; set; }
        public DateTime GenerateTime { get; set; }
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public string TranposrtName { get; set; }
        public decimal Version { get; set; }
    }
}