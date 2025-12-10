namespace TransactionService.Api.Dtos;

public record TransactionResponseDto(
    Guid Id,
    Guid MerchantId,
    decimal Amount,
    string Currency,
    string Direction,
    DateTime OccurredAt
);
