namespace ConsolidationService.Api.Dtos;

public record DailyBalanceResponseDto(
    string MerchantId,
    DateTime Date,
    decimal TotalCredit,
    decimal TotalDebit,
    decimal Balance
);
