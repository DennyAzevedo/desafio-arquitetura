using ConsolidationService.Api.Dtos;
using ConsolidationService.Application.Queries;

namespace ConsolidationService.Application.Services;

public class ConsolidationQueryService
{
    private readonly IDailyBalanceRepository _repository;

    public ConsolidationQueryService(IDailyBalanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailyBalanceResponseDto?> GetDailyBalanceAsync(GetDailyBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var dailyBalance = await _repository.GetDailyBalanceAsync(query.MerchantId, query.Date.Date, cancellationToken);
        
        if (dailyBalance == null)
            return null;

        return new DailyBalanceResponseDto(
            dailyBalance.MerchantId,
            dailyBalance.Date,
            dailyBalance.TotalCredit,
            dailyBalance.TotalDebit,
            dailyBalance.Balance
        );
    }
}
