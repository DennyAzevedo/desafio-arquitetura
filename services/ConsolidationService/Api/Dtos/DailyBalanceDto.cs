namespace ConsolidationService.Api.Dtos;

public record DailyBalanceResponseDto(
    string MerchantId,
    DateOnly Date,
    decimal TotalCredit,
    decimal TotalDebit,
    decimal Balance
);
