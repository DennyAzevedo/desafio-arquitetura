using ConsolidationService.Domain.Entities;

namespace ConsolidationService.Application.Services;

public interface IDailyBalanceRepository
{
    Task<DailyBalance?> GetDailyBalanceAsync(string merchantId, DateTime date, CancellationToken cancellationToken = default);
}
