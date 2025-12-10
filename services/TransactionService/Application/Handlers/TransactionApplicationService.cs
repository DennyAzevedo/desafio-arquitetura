using TransactionService.Application.Commands;
using TransactionService.Application.Services;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Handlers;

public class TransactionApplicationService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionApplicationService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionCommand command, CancellationToken cancellationToken = default)
    {
        var transaction = new Transaction(
            command.MerchantId,
            command.Amount,
            command.Currency,
            command.Direction,
            command.OccurredAt
        );

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return transaction.Id;
    }
}
