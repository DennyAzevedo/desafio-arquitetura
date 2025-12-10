using ConsolidationService.Domain.Entities;
using ConsolidationService.Infrastructure.Persistence;
using ConsolidationService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ConsolidationService.Tests.Integration;

[Trait("Category", "Integration")]
public class DailyBalanceRepositoryTests : IDisposable
{
    private readonly ReadDbContext _context;
    private readonly DailyBalanceRepository _repository;

    public DailyBalanceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ReadDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ReadDbContext(options);
        _repository = new DailyBalanceRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistDailyBalance()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var dailyBalance = new DailyBalance(merchantId, date);
        dailyBalance.AddCredit(100m);

        await _repository.AddAsync(dailyBalance);

        var result = await _repository.GetByMerchantAndDateAsync(merchantId, date);
        result.Should().NotBeNull();
        result!.TotalCredit.Should().Be(100m);
    }

    [Fact]
    public async Task GetByMerchantAndDateAsync_WhenNotExists_ShouldReturnNull()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;

        var result = await _repository.GetByMerchantAndDateAsync(merchantId, date);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingDailyBalance()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var dailyBalance = new DailyBalance(merchantId, date);
        dailyBalance.AddCredit(100m);
        await _repository.AddAsync(dailyBalance);

        var existing = await _repository.GetByMerchantAndDateAsync(merchantId, date);
        existing!.AddDebit(30m);
        await _repository.UpdateAsync(existing);

        var result = await _repository.GetByMerchantAndDateAsync(merchantId, date);
        result!.TotalCredit.Should().Be(100m);
        result.TotalDebit.Should().Be(30m);
        result.Balance.Should().Be(70m);
    }

    [Fact]
    public async Task GetByMerchantAndDateAsync_ShouldReturnCorrectRecord()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;
        var dailyBalance = new DailyBalance(merchantId, date);
        dailyBalance.AddCredit(250m);
        dailyBalance.AddDebit(50m);
        await _repository.AddAsync(dailyBalance);

        var result = await _repository.GetByMerchantAndDateAsync(merchantId, date);

        result.Should().NotBeNull();
        result!.MerchantId.Should().Be(merchantId);
        result.Date.Should().Be(date);
        result.TotalCredit.Should().Be(250m);
        result.TotalDebit.Should().Be(50m);
        result.Balance.Should().Be(200m);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
