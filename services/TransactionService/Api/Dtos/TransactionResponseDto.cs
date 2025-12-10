namespace TransactionService.Api.Dtos;

public record TransactionResponseDto(
    Guid Id,
    string MerchantId,
    decimal Amount,
    string Currency,
    string Direction,
    DateOnly OccurredAt,
    DateTime CreatedAt
);
