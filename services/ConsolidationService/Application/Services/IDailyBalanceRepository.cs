using ConsolidationService.Domain.Entities;

namespace ConsolidationService.Application.Services;

public interface IDailyBalanceRepository
{
    Task<DailyBalance?> GetByMerchantAndDateAsync(Guid merchantId, DateTime date);
    Task AddAsync(DailyBalance dailyBalance);
    Task UpdateAsync(DailyBalance dailyBalance);
}
