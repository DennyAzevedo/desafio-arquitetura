using ConsolidationService.Application.Services;
using ConsolidationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsolidationService.Infrastructure.Persistence.Repositories;

public class DailyBalanceRepository : IDailyBalanceRepository
{
    private readonly ReadDbContext _context;

    public DailyBalanceRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<DailyBalance?> GetByMerchantAndDateAsync(Guid merchantId, DateTime date)
    {
        return await _context.DailyBalances
            .FirstOrDefaultAsync(x => x.MerchantId == merchantId && x.Date == date.Date);
    }

    public async Task AddAsync(DailyBalance dailyBalance)
    {
        await _context.DailyBalances.AddAsync(dailyBalance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DailyBalance dailyBalance)
    {
        _context.DailyBalances.Update(dailyBalance);
        await _context.SaveChangesAsync();
    }
}
