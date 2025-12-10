namespace ConsolidationService.Application.Queries;

public record GetDailyBalanceQuery(Guid MerchantId, DateTime Date);
