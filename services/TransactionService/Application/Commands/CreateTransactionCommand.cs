using TransactionService.Domain.Enums;

namespace TransactionService.Application.Commands;

public record CreateTransactionCommand(
    Guid MerchantId,
    decimal Amount,
    string Currency,
    TransactionDirection Direction,
    DateTime OccurredAt
);
