using System;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class ErrorOccuredWhenProccessTranposrtXml : Exception
    {
        public string ParsedMessage { get; }

        public ErrorOccuredWhenProccessTranposrtXml(string parsedMessage, Exception innerException)
            : base($"There was error when try parse message {parsedMessage}. Error was {innerException.Message}. Please check innerException for more details", innerException)
        {
            ParsedMessage = parsedMessage;
        }
    }
}