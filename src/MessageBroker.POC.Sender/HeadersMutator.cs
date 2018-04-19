using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
using Console = System.Console;

namespace MessageBroker.POC.Sender
{
    public class IncomingHeadersMutator : IMutateIncomingMessages
    {
        private ILog _log = LogManager.GetLogger<IncomingHeadersMutator>();
  

        public Task MutateIncoming(MutateIncomingMessageContext context)
        {
            //foreach (var contextOutgoingHeader in context.Headers)
            //{
            //    Console.WriteLine($"Header key: {contextOutgoingHeader.Key} value: {contextOutgoingHeader.Value}");
            //    _log.Debug($"Header key: {contextOutgoingHeader.Key} value: {contextOutgoingHeader.Value}");
            //}

            return Task.CompletedTask;
        }


    }

    public class OutComingHeadersMutator : IMutateOutgoingMessages
    {
        private ILog _log = LogManager.GetLogger<OutComingHeadersMutator>();
        public Task MutateOutgoing(MutateOutgoingMessageContext context)
        {
            //foreach (var contextOutgoingHeader in context.OutgoingHeaders)
            //{
            //    Console.WriteLine($"Header key: {contextOutgoingHeader.Key} value: {contextOutgoingHeader.Value}");
            //    _log.Debug($"Header key: {contextOutgoingHeader.Key} value: {contextOutgoingHeader.Value}");
            //}

            return Task.CompletedTask;
        }
    }
}
