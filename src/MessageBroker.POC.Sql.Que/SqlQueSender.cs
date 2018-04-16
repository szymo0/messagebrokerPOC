using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Messaging;

namespace MessageBroker.POC.Sql.Que
{
    public class SqlQueSender
    {

        private static void CreateIfNotExists(string queueName)
        {
            if (!MessageQueue.Exists(queueName))
            {
                var que=MessageQueue.Create(queueName, true);
            }
        }

        [SqlProcedure]

        public static void Send(SqlString queueName, SqlString message,SqlInt32 priority,SqlInt32 timeToLive)
        {
            if (queueName == null || string.IsNullOrEmpty(queueName.Value))
                throw new Exception("Message queue name need to be provided");

            var queue = queueName.Value;
            var queueDeadLetter = queue + "DeadLetter";
            CreateIfNotExists(queue);
            CreateIfNotExists(queueDeadLetter);
            try
            {
                using (var messageQueue = new MessageQueue(queue, QueueAccessMode.Send))
                {
                    Message messageMsqm=new Message();
                    if(!priority.IsNull)
                        messageMsqm.Priority = (MessagePriority) priority.Value;

                    messageMsqm.Body = message.Value;
                    messageMsqm.AcknowledgeType = AcknowledgeTypes.PositiveArrival | AcknowledgeTypes.PositiveReceive;
                    messageMsqm.AttachSenderId = true;
                    messageMsqm.UseDeadLetterQueue = true;

                    if(!timeToLive.IsNull)
                        messageMsqm.TimeToBeReceived=new TimeSpan(0,0,0,timeToLive.Value,0);
                    else
                        messageMsqm.TimeToBeReceived=new TimeSpan(0,0,5,0,0);

                    messageMsqm.UseDeadLetterQueue = true;
                    messageMsqm.UseJournalQueue = true;
                    messageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
                    if(messageQueue.Transactional)
                        messageQueue.Send(messageMsqm,MessageQueueTransactionType.Single);
                    else
                        messageQueue.Send(messageMsqm);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [SqlProcedure]
        public static void SendSimple(SqlString queueName, SqlString message)
        {
           Send(queueName,message,new SqlInt32(),new SqlInt32());
        }

        [SqlProcedure]
        public static SqlInt32 CountMessageOnQue(SqlString queueName)
        {
            if (!MessageQueue.Exists(queueName.Value))
                return new SqlInt32(0);

            MessageQueue messageQueue=new MessageQueue(queueName.Value,QueueAccessMode.PeekAndAdmin);
            MessageEnumerator messageEnumerator = messageQueue.GetMessageEnumerator2();
            int i = 0;
            while (messageEnumerator.MoveNext())
            {
                i++;
            }
            return new SqlInt32(i);
        }   

    }
}
