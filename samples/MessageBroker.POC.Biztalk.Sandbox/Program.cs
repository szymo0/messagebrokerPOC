using System;
using System.Collections.Generic;
using System.IO;
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
            if (!MessageQueue.Exists($@"{Environment.MachineName}\Private$\FromSql"))
            {
                MessageQueue.Create($@"{Environment.MachineName}\Private$\FromSql", true);
            }
            MessageQueue rQueue = new MessageQueue($@"{Environment.MachineName}\Private$\FromSql");
            rQueue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
            rQueue.UseJournalQueue = true;
            foreach (var file in Directory.EnumerateFiles(@"c:\MSMQ_transport_xmls"))
            {
                rQueue.Send(File.ReadAllText(file),MessageQueueTransactionType.Single);
            }


            //SqlQueSender.Send($@"{Environment.MachineName}\Private$\Test", "message " + DateTime.Now.ToString(), 7, 4);
            //SqlQueSender.SendSimple($@"{Environment.MachineName}\Private$\TestQUE", "message " + DateTime.Now.ToString());

            Console.WriteLine("Send all");
            Console.WriteLine($@"{Environment.MachineName}\Private$\FromSql message count: " + SqlQueSender.CountMessageOnQue($@"{Environment.MachineName}\Private$\FromSql"));
            Console.WriteLine($@"{Environment.MachineName}\DeadLetter$  message count: " + SqlQueSender.CountMessageOnQue($@"FormatName:DIRECT=OS:{Environment.MachineName}\System$;Deadletter"));
            Console.WriteLine($@"{Environment.MachineName}\DEADXACT message count: " + SqlQueSender.CountMessageOnQue($@"FormatName:DIRECT=OS:{Environment.MachineName}\System$;Deadxact"));
            Console.ReadKey();

        }
    }
}
