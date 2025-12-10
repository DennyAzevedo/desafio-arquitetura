namespace ConsolidationService.Api.Dtos;

public record DailyBalanceDto(
    Guid MerchantId,
    DateTime Date,
    decimal TotalCredit,
    decimal TotalDebit,
    decimal Balance
);
