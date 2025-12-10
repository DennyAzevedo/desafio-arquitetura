using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid MerchantId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public TransactionDirection Direction { get; private set; }
    public DateTime OccurredAt { get; private set; }

    private Transaction() { }

    public Transaction(Guid merchantId, decimal amount, string currency, TransactionDirection direction, DateTime occurredAt)
    {
        Id = Guid.NewGuid();
        MerchantId = merchantId;
        Amount = amount;
        Currency = currency;
        Direction = direction;
        OccurredAt = occurredAt;
    }
}
