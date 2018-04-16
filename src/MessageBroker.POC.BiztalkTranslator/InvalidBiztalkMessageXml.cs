using System;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class InvalidBiztalkMessageXml : Exception
    {
        public InvalidBiztalkMessageXml(string biztalkXml)
            : base($"Invalid biztalkXml {biztalkXml}")
        {
            BiztalkXml = biztalkXml;
        }

        public string BiztalkXml { get; }
    }
}