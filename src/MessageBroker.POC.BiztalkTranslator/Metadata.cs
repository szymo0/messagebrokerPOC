using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class Metadata
    {
        public IEnumerable<string> Destinations { get; internal set; }
        public DateTime? GenerateDate { get; internal set; }
        public string BussinesId { get; set; }
        public Guid CorrelationId { get; set; }
        public string AttachmentId { get; set; }
        public string InsertMethodName { get; set; }
        public string ConfirmMethodName { get; set; }
        public string Source { get; set; }
        public IList<DestinationMetadata> GenerateForDestinations()
        {
            List<DestinationMetadata> metadatas = new List<DestinationMetadata>();
            foreach (var destination in Destinations)
            {
                metadatas.Add(new DestinationMetadata
                {
                    Destination = destination,
                    GenerateDate = GenerateDate,
                    InsertMethodName = InsertMethodName,
                    CorrelationId = CorrelationId,
                    BussinesId = BussinesId,
                    Source = Source
                });
            }

            return metadatas;
        }


    }

    public class DestinationMetadata
    {
        public string Source { get; internal set; }
        public string Destination { get; internal set; }
        public DateTime? GenerateDate { get; internal set; }
        public string InsertMethodName { get; set; }
        public string BussinesId { get; set; }
        public Guid CorrelationId { get; set; }

    }

}