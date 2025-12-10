using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Services;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly TransactionDbContext _context;

    public OutboxRepository(TransactionDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxEvent outboxEvent)
    {
        await _context.OutboxEvents.AddAsync(outboxEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OutboxEvent>> GetPendingEventsAsync()
    {
        return await _context.OutboxEvents
            .Where(x => x.Status == "Pending")
            .OrderBy(x => x.OccurredOn)
            .Take(10)
            .ToListAsync();
    }

    public async Task UpdateAsync(OutboxEvent outboxEvent)
    {
        _context.OutboxEvents.Update(outboxEvent);
        await _context.SaveChangesAsync();
    }
}
