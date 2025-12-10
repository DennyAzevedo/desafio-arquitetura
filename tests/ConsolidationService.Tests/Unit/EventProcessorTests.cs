using ConsolidationService.Application.Handlers;
using ConsolidationService.Application.Services;
using ConsolidationService.Domain.Entities;
using ConsolidationService.Infrastructure.Messaging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConsolidationService.Tests.Unit;

public class EventProcessorTests
{
    private readonly Mock<IDailyBalanceRepository> _repositoryMock;
    private readonly Mock<ILogger<EventProcessor>> _loggerMock;
    private readonly EventProcessor _processor;

    public EventProcessorTests()
    {
        _repositoryMock = new Mock<IDailyBalanceRepository>();
        _loggerMock = new Mock<ILogger<EventProcessor>>();
        _processor = new EventProcessor(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ProcessTransactionCreatedAsync_WithCreditDirection_ShouldAddCredit()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var amount = 100m;

        var @event = new TransactionCreatedEvent(
            Guid.NewGuid(),
            merchantId,
            "CREDIT",
            amount,
            date
        );

        var dailyBalance = new DailyBalance(merchantId, date);
        _repositoryMock.Setup(x => x.GetByMerchantAndDateAsync(merchantId, date))
            .ReturnsAsync(dailyBalance);

        await _processor.ProcessTransactionCreatedAsync(@event);

        _repositoryMock.Verify(x => x.UpdateAsync(It.Is<DailyBalance>(
            db => db.TotalCredit == amount && db.TotalDebit == 0 && db.Balance == amount
        )), Times.Once);
    }

    [Fact]
    public async Task ProcessTransactionCreatedAsync_WithDebitDirection_ShouldAddDebit()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var amount = 50m;

        var @event = new TransactionCreatedEvent(
            Guid.NewGuid(),
            merchantId,
            "DEBIT",
            amount,
            date
        );

        var dailyBalance = new DailyBalance(merchantId, date);
        _repositoryMock.Setup(x => x.GetByMerchantAndDateAsync(merchantId, date))
            .ReturnsAsync(dailyBalance);

        await _processor.ProcessTransactionCreatedAsync(@event);

        _repositoryMock.Verify(x => x.UpdateAsync(It.Is<DailyBalance>(
            db => db.TotalDebit == amount && db.TotalCredit == 0 && db.Balance == -amount
        )), Times.Once);
    }

    [Fact]
    public async Task ProcessTransactionCreatedAsync_WhenDailyBalanceDoesNotExist_ShouldCreateNew()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var amount = 200m;

        var @event = new TransactionCreatedEvent(
            Guid.NewGuid(),
            merchantId,
            "CREDIT",
            amount,
            date
        );

        _repositoryMock.Setup(x => x.GetByMerchantAndDateAsync(merchantId, date))
            .ReturnsAsync((DailyBalance?)null);

        await _processor.ProcessTransactionCreatedAsync(@event);

        _repositoryMock.Verify(x => x.AddAsync(It.Is<DailyBalance>(
            db => db.MerchantId == merchantId && db.TotalCredit == amount
        )), Times.Once);
    }
}
