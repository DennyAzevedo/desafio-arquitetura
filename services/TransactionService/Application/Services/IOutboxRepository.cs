using TransactionService.Domain.Entities;

namespace TransactionService.Application.Services;

public interface IOutboxRepository
{
    Task AddAsync(OutboxEvent outboxEvent);
    Task<List<OutboxEvent>> GetPendingEventsAsync();
    Task UpdateAsync(OutboxEvent outboxEvent);
}
