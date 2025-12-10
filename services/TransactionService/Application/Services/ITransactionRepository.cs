using TransactionService.Domain.Entities;

namespace TransactionService.Application.Services;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
}
