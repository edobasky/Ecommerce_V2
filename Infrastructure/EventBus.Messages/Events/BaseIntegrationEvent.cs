namespace EventBus.Messages.Events
{
    public class BaseIntegrationEvent
    {
        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public BaseIntegrationEvent(Guid correlationId, DateTime creationDate)
        {
            CorrelationId = correlationId;
            CreationDate = creationDate;
        }

        public Guid CorrelationId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
