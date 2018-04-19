using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MessageBroker.POC.Messages.Messages;

namespace MessageBroker.POC.BiztalkTranslator
{
    public interface IRowParser
    {
        IEnumerable<BiztalkMessage> ParseXmlForRows(XElement message);
    }
    public class BiztalkRowParser : IRowParser
    {
        private readonly IMetadataParser _metadataPareser;

        public BiztalkRowParser(IMetadataParser metadataPareser)
        {
            _metadataPareser = metadataPareser;
        }
        public IEnumerable<BiztalkMessage> ParseXmlForRows(XElement message)
        {
            List<BiztalkMessage> messages = new List<BiztalkMessage>();

            if (message == null || message.IsEmpty)
                return messages;


            var metaDataXmlElement = message.Element("MetaData");
            var metaData = _metadataPareser.ParseFromXElementToMetadata(metaDataXmlElement);
            var dataToTransport = (metaDataXmlElement.NextNode as XElement);
            var desitnationsMetaData = metaData.GenerateForDestinations();
            foreach (var destMetadata in desitnationsMetaData)
            {
                XElement messageData = new XElement(message.Name.LocalName);
                messageData.Add(message.Attributes().ToArray());
                messageData.Add(_metadataPareser.ParseToXmlElement(destMetadata));
                messageData.Add(dataToTransport);

                BiztalkMessage biztalkMessage = new BiztalkMessage();
                biztalkMessage.Data = messageData.ToString(SaveOptions.None);
                biztalkMessage.CorrelationId = metaData.CorrelationId;
                biztalkMessage.BussinesId = metaData.BussinesId;
                biztalkMessage.Source = metaData.Source;
                biztalkMessage.GenerateTime = DateTime.Now;
                biztalkMessage.Destination = destMetadata.Destination;
                biztalkMessage.TranposrtName = message.Name.LocalName;
                messages.Add(biztalkMessage);
            }


            return messages;
        }
    }
}