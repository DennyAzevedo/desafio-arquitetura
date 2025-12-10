namespace ConsolidationService.Application.Handlers;

public record TransactionCreatedEvent(
    Guid TransactionId,
    Guid MerchantId,
    string Direction,
    decimal Amount,
    DateTime OccurredAt
);
