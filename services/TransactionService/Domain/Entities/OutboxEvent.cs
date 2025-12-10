namespace TransactionService.Domain.Entities;

public class OutboxEvent
{
    public Guid Id { get; private set; }
    public Guid AggregateId { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public string Status { get; private set; }

    private OutboxEvent() { }

    public OutboxEvent(Guid aggregateId, string type, string payload)
    {
        Id = Guid.NewGuid();
        AggregateId = aggregateId;
        Type = type;
        Payload = payload;
        OccurredOn = DateTime.UtcNow;
        Status = "Pending";
    }

    public void MarkAsProcessed()
    {
        Status = "Processed";
        ProcessedOn = DateTime.UtcNow;
    }
}
