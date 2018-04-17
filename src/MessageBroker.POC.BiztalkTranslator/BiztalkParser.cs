using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MessageBroker.POC.Messages.Messages;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class BiztalkParser
    {
        private readonly IRowParser _rowParser;

        public BiztalkParser(IRowParser rowParser)
        {
            _rowParser = rowParser;
        }

        public IEnumerable<BiztalkMessage> ParseMessage(string biztalkMessage)
        {
            List<BiztalkMessage> list = new List<BiztalkMessage>();


            if (string.IsNullOrEmpty(biztalkMessage) )
                throw new InvalidBiztalkMessageXml(biztalkMessage);

            var message = XDocument.Parse(biztalkMessage);

            if (message?.Root == null || message.Root.IsEmpty || (string.IsNullOrEmpty(message.Root.Value) && !message.Root.HasElements))
                throw new InvalidBiztalkMessageXml(biztalkMessage);
            try
            {
                return _rowParser.ParseXmlForRows(message.Root);
            }
            catch (Exception ex)
            {
                throw new ErrorOccuredWhenProccessTranposrtXml(biztalkMessage, ex);
            }

        }

    }
}
