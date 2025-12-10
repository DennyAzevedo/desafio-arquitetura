using System.Text.Json;
using TransactionService.Application.Commands;
using TransactionService.Application.Services;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Handlers;

public class TransactionApplicationService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IOutboxRepository _outboxRepository;

    public TransactionApplicationService(ITransactionRepository transactionRepository, IOutboxRepository outboxRepository)
    {
        _transactionRepository = transactionRepository;
        _outboxRepository = outboxRepository;
    }

    public async Task<Transaction> CreateTransactionAsync(CreateTransactionCommand command)
    {
        var transaction = new Transaction(
            command.MerchantId,
            command.Amount,
            command.Currency,
            command.Direction,
            command.OccurredAt
        );

        await _transactionRepository.AddAsync(transaction);

        var eventPayload = JsonSerializer.Serialize(new
        {
            TransactionId = transaction.Id,
            MerchantId = transaction.MerchantId,
            Direction = transaction.Direction.ToString().ToUpper(),
            Amount = transaction.Amount,
            OccurredAt = transaction.OccurredAt
        });

        var outboxEvent = new OutboxEvent(transaction.Id, "TransactionCreated", eventPayload);
        await _outboxRepository.AddAsync(outboxEvent);

        return transaction;
    }
}
