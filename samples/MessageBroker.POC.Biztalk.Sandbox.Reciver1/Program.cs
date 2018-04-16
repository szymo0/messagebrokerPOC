using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.POC.Biztalk.Sandbox.Reciver1
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageQueue rQueue = new MessageQueue($@"{Environment.MachineName}\Private$\Test");
            rQueue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
            rQueue.BeginReceive();
            rQueue.ReceiveCompleted += (sender, eventArgs) =>
            {
                var cmq = ((MessageQueue)sender);
                cmq.EndReceive(eventArgs.AsyncResult);
                Console.WriteLine(eventArgs.Message.Body);
                cmq.Refresh();
                cmq.BeginReceive();

            };
            Console.ReadKey();
        }
    }
}
