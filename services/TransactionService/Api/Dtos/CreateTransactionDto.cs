namespace TransactionService.Api.Dtos;

public record CreateTransactionRequestDto(
    string MerchantId,
    decimal Amount,
    string Currency,
    string Direction,
    DateTime OccurredAt
);
