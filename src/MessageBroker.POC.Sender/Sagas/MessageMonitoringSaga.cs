using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBroker.POC.Messages.Messages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

namespace MessageBroker.POC.Sender.Sagas
{
    public class MessageMonitoringSaga : NServiceBus.Persistence.Sql.SqlSaga<MessageMonitor>,
        IAmStartedByMessages<TransportMessageSend>,
        IHandleMessages<TransportMessageRecived>,
        IHandleMessages<TransportMessageCompleted>
    {
        private ILog _logger = LogManager.GetLogger<MessageMonitoringSaga>();
        //protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MessageMonitor> mapper)
        //{
          
        //}
        public Task Handle(TransportMessageSend message, IMessageHandlerContext context)
        {
            _logger.Info("Handle transportMesage");
            Data.BussinesId = message.BussinesId;
            Data.CorrelationId = message.CorrelationId;
            Data.SendDate = DateTime.Now;
            Data.IsSend = true;

            return Task.CompletedTask;


        }


        public Task Handle(TransportMessageRecived message, IMessageHandlerContext context)
        {
            Data.IsRecived = true;
            Data.RecivedDate=DateTime.Now;
            return Task.CompletedTask;
        }

        public Task Handle(TransportMessageCompleted message, IMessageHandlerContext context)
        {
            Data.IsProcessed = true;
            Data.ProcessedDate=DateTime.Now;
            this.MarkAsComplete();
            return Task.CompletedTask;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<TransportMessageSend>(c => c.CorrelationId);
            mapper.ConfigureMapping<TransportMessageRecived>(c => c.CorrelationId);
            mapper.ConfigureMapping<TransportMessageCompleted>(c => c.CorrelationId);
        }

        protected override string CorrelationPropertyName => nameof(Data.CorrelationId);
    }

    public class MessageMonitor : IContainSagaData
    {
        public bool IsSend { get; set; }
        public bool IsRecived { get; set; }
        public bool IsProcessed { get; set; }

        public DateTime SendDate { get; set; }
        public DateTime RecivedDate { get; set; }
        public DateTime ProcessedDate { get; set; }

        public string BussinesId { get; set; }
        public Guid CorrelationId { get; set; }

        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }
}
