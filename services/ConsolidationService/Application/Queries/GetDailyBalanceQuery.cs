namespace ConsolidationService.Application.Queries;

public record GetDailyBalanceQuery(string MerchantId, DateOnly Date);
