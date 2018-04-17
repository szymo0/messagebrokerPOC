using System;
using System.Threading.Tasks;
using NServiceBus;

namespace MessageBroker.POC.BiztalkTranslator
{
    public class Program
    {
        static void Main(string[] args)
        {

            Task.Run(() =>
            {
                Processor processor = new Processor();
                processor.StartProcess();
            });

            Console.WriteLine("Started");
            Console.ReadLine();


        }
    }
}
