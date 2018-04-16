using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
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

            MessageQueue.Exists(@"FormatName:DIRECT=tcp:127.0.0.1\DEADLETTER$");
            foreach(var que in MessageQueue.GetPrivateQueuesByMachine("RUBGHYNXYY"))
            {
                Console.WriteLine(que.FormatName);
            }

            Console.WriteLine();

            //foreach (var que in MessageQueue.GetPublicQueuesByMachine("RUBGHYNXYY"))
            //{
            //    Console.WriteLine(que.FormatName);
            //}

            //Console.WriteLine();

            //var enumerator =
            //    MessageQueue.GetMessageQueueEnumerator(new MessageQueueCriteria() {MachineName = "RUBGHYNXYY"});

            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current.FormatName);
            //}


            // SqlQueSender.SendSimple(@"RUBGHYNXYY\Private$\Test", "message "+DateTime.Now.ToString()); 
            //SqlQueSender.SendSimple(@"RUBGHYNXYY\Private$\TestQUE", "message " + DateTime.Now.ToString());
            //Console.WriteLine(@"RUBGHYNXYY\Private$\Test message count: " + SqlQueSender.CountMessageOnQue(@"RUBGHYNXYY\Private$\Test"));
            //Console.WriteLine(@"RUBGHYNXYY\Private$\TestQUE message count: " + SqlQueSender.CountMessageOnQue(@"RUBGHYNXYY\Private$\TestQUE"));
            Console.WriteLine(@"RUBGHYNXYY\DeadLetter$  message count: " + SqlQueSender.CountMessageOnQue($@"DIRECT=OS:{Environment.MachineName}\SYSTEM$;DEADLETTER"));
            Console.WriteLine(@"RUBGHYNXYY\DEADXACT message count: " + SqlQueSender.CountMessageOnQue(@"RUBGHYNXYY\DEADXACT"));
            Console.ReadKey();

        }
    }
}
