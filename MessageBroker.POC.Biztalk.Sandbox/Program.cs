using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Sql.Que;
using Console = System.Console;

namespace MessageBroker.POC.Biztalk.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //MessageQueue messageQueue=new MessageQueue($@"FormatName:DIRECT=OS:{Environment.MachineName}\System$;Deadxact", QueueAccessMode.ReceiveAndAdmin);
            //messageQueue.Formatter= new XmlMessageFormatter(new[] { typeof(string) });
            //messageQueue.MessageReadPropertyFilter.Body = true;
            //messageQueue.MessageReadPropertyFilter.DestinationQueue = true;
            //messageQueue.MessageReadPropertyFilter.Label = true;

            //using (var enumerator= messageQueue.GetMessageEnumerator2())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        Console.WriteLine(enumerator.Current.Body +" " +enumerator.Current.DestinationQueue.FormatName);

            //    } 
            //}

            //Task.Run(() =>
            //{
            //        MessageQueue rQueue = new MessageQueue($@"{Environment.MachineName}\Private$\Test");
            //        rQueue.Formatter = new XmlMessageFormatter(new[] {typeof(string)});
            //        rQueue.BeginReceive();
            //        rQueue.ReceiveCompleted += (sender, eventArgs) =>
            //        {
            //            var cmq = ((MessageQueue) sender);
            //            cmq.EndReceive(eventArgs.AsyncResult);
            //            //Console.WriteLine(eventArgs.Message.Body);
            //            cmq.Refresh();
            //            cmq.BeginReceive();

            //        };
            //});

            for (int i = 0; i < 1000; i++)
            {
                MessageQueue rQueue = new MessageQueue($@"{Environment.MachineName}\Private$\Test");
                rQueue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
                rQueue.UseJournalQueue = true;
                rQueue.Send("My date "+DateTime.Now,MessageQueueTransactionType.Single);
            };
            //SqlQueSender.Send($@"{Environment.MachineName}\Private$\Test", "message " + DateTime.Now.ToString(), 7, 4);
            //SqlQueSender.SendSimple($@"{Environment.MachineName}\Private$\TestQUE", "message " + DateTime.Now.ToString());

            Console.WriteLine("Send all");
            Console.WriteLine($@"{Environment.MachineName}\Private$\Test message count: " + SqlQueSender.CountMessageOnQue($@"{Environment.MachineName}\Private$\Test"));
            Console.WriteLine($@"{Environment.MachineName}\Private$\TestQUE message count: " + SqlQueSender.CountMessageOnQue($@"{Environment.MachineName}\Private$\TestQUE"));
            Console.WriteLine($@"{Environment.MachineName}\DeadLetter$  message count: " + SqlQueSender.CountMessageOnQue($@"FormatName:DIRECT=OS:{Environment.MachineName}\System$;Deadletter"));
            Console.WriteLine($@"{Environment.MachineName}\DEADXACT message count: " + SqlQueSender.CountMessageOnQue($@"FormatName:DIRECT=OS:{Environment.MachineName}\System$;Deadxact"));
            Console.ReadKey();

        }
    }
}
