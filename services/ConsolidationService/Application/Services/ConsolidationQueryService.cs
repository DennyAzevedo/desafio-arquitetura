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

    public async Task<DailyBalanceDto?> GetDailyBalanceAsync(GetDailyBalanceQuery query)
    {
        var dailyBalance = await _repository.GetByMerchantAndDateAsync(query.MerchantId, query.Date.Date);
        
        if (dailyBalance == null)
            return null;

        return new DailyBalanceDto(
            dailyBalance.MerchantId,
            dailyBalance.Date,
            dailyBalance.TotalCredit,
            dailyBalance.TotalDebit,
            dailyBalance.Balance
        );
    }
}
