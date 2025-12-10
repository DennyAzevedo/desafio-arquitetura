using TransactionService.Domain.Enums;

namespace TransactionService.Application.Commands;

public record CreateTransactionCommand(
    string MerchantId,
    decimal Amount,
    string Currency,
    TransactionDirection Direction,
    DateTime OccurredAt
);
