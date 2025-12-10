namespace TransactionService.Api.Dtos;

public record CreateTransactionDto(
    Guid MerchantId,
    decimal Amount,
    string Currency,
    string Direction,
    DateTime OccurredAt
);
