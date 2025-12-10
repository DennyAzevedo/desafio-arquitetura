using ConsolidationService.Infrastructure.Persistence;
using ConsolidationService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using static ConsolidationService.Infrastructure.Persistence.ReadDbContext;

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
        
        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetDailyBalanceAsync_WhenNoTransactions_ShouldReturnNull()
    {
        var merchantId = "merchant123";
        var date = DateTime.UtcNow.Date;

        var result = await _repository.GetDailyBalanceAsync(merchantId, date);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetDailyBalanceAsync_WithTransactions_ShouldCalculateCorrectly()
    {
        var merchantId = "merchant123";
        var date = DateTime.UtcNow.Date;
        
        // Add test transactions
        await AddTransactionAsync(merchantId, 100m, 0, date); // Credit
        await AddTransactionAsync(merchantId, 50m, 0, date);  // Credit
        await AddTransactionAsync(merchantId, 30m, 1, date);  // Debit

        var result = await _repository.GetDailyBalanceAsync(merchantId, date);

        result.Should().NotBeNull();
        result!.MerchantId.Should().Be(merchantId);
        result.Date.Should().Be(date);
        result.TotalCredit.Should().Be(150m);
        result.TotalDebit.Should().Be(30m);
        result.Balance.Should().Be(120m);
    }

    [Fact]
    public async Task GetDailyBalanceAsync_WithDifferentDates_ShouldFilterByDate()
    {
        var merchantId = "merchant123";
        var today = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);
        
        await AddTransactionAsync(merchantId, 100m, 0, today);
        await AddTransactionAsync(merchantId, 200m, 0, yesterday);

        var result = await _repository.GetDailyBalanceAsync(merchantId, today);

        result.Should().NotBeNull();
        result!.TotalCredit.Should().Be(100m);
    }

    private async Task AddTransactionAsync(string merchantId, decimal amount, int direction, DateTime date)
    {
        var transaction = new TransactionEntity
        {
            Id = Guid.NewGuid(),
            MerchantId = merchantId,
            Amount = amount,
            Currency = "BRL",
            Direction = direction,
            OccurredAt = date
        };

        _context.Set<TransactionEntity>().Add(transaction);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
