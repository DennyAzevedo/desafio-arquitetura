using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public string MerchantId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public TransactionDirection Direction { get; private set; }
    public DateOnly OccurredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction() { MerchantId = string.Empty; Currency = string.Empty; }

    public Transaction(string merchantId, decimal amount, string currency, TransactionDirection direction, DateOnly occurredAt)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
        if (string.IsNullOrWhiteSpace(merchantId))
            throw new ArgumentException("MerchantId is required", nameof(merchantId));

        Id = Guid.NewGuid();
        MerchantId = merchantId;
        Amount = amount;
        Currency = currency;
        Direction = direction;
        OccurredAt = occurredAt;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsCredit() => Direction == TransactionDirection.Credit;
    public bool IsDebit() => Direction == TransactionDirection.Debit;
}
