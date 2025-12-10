using ConsolidationService.Application.Services;
using ConsolidationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static ConsolidationService.Infrastructure.Persistence.ReadDbContext;

namespace ConsolidationService.Infrastructure.Persistence.Repositories;

public class DailyBalanceRepository : IDailyBalanceRepository
{
    private readonly ReadDbContext _context;

    public DailyBalanceRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<DailyBalance?> GetDailyBalanceAsync(string merchantId, DateTime date, CancellationToken cancellationToken = default)
    {
        var targetDate = date.Date;
        
        var transactions = await _context.Set<TransactionEntity>()
            .Where(t => t.MerchantId == merchantId && t.OccurredAt.Date == targetDate)
            .ToListAsync(cancellationToken);

        if (!transactions.Any())
            return null;

        var totalCredit = transactions.Where(t => t.Direction == 0).Sum(t => t.Amount);
        var totalDebit = transactions.Where(t => t.Direction == 1).Sum(t => t.Amount);

        return new DailyBalance(merchantId, targetDate, totalCredit, totalDebit);
    }
}
