using ConsolidationService.Application.Handlers;
using ConsolidationService.Application.Services;
using ConsolidationService.Domain.Entities;

namespace ConsolidationService.Infrastructure.Messaging;

public class EventProcessor
{
    private readonly IDailyBalanceRepository _repository;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(IDailyBalanceRepository repository, ILogger<EventProcessor> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ProcessTransactionCreatedAsync(TransactionCreatedEvent @event)
    {
        var date = @event.OccurredAt.Date;
        var dailyBalance = await _repository.GetByMerchantAndDateAsync(@event.MerchantId, date);

        if (dailyBalance == null)
        {
            dailyBalance = new DailyBalance(@event.MerchantId, date);
            
            if (@event.Direction.ToUpper() == "CREDIT")
                dailyBalance.AddCredit(@event.Amount);
            else if (@event.Direction.ToUpper() == "DEBIT")
                dailyBalance.AddDebit(@event.Amount);

            await _repository.AddAsync(dailyBalance);
            _logger.LogInformation("Created new DailyBalance for Merchant {MerchantId} on {Date}", @event.MerchantId, date);
        }
        else
        {
            if (@event.Direction.ToUpper() == "CREDIT")
                dailyBalance.AddCredit(@event.Amount);
            else if (@event.Direction.ToUpper() == "DEBIT")
                dailyBalance.AddDebit(@event.Amount);

            await _repository.UpdateAsync(dailyBalance);
            _logger.LogInformation("Updated DailyBalance for Merchant {MerchantId} on {Date}", @event.MerchantId, date);
        }
    }
}
