using ConsolidationService.Domain.Entities;

namespace ConsolidationService.Application.Services;

public interface IDailyBalanceRepository
{
    Task<DailyBalance?> GetDailyBalanceAsync(string merchantId, DateOnly date, CancellationToken cancellationToken = default);
}
