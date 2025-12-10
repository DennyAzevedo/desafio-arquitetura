using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;

namespace TransactionService.Tests.Integration;

[Trait("Category", "Integration")]
public class TransactionRepositoryTests : IDisposable
{
    private readonly TransactionDbContext _context;
    private readonly TransactionRepository _repository;

    public TransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TransactionDbContext(options);
        _repository = new TransactionRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistTransaction()
    {
        // Arrange
        var transaction = new Transaction(
            "merchant123",
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateOnly.FromDateTime(DateTime.UtcNow)
        );

        // Act
        await _repository.AddAsync(transaction);

        // Assert
        var savedTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
        savedTransaction.Should().NotBeNull();
        savedTransaction!.MerchantId.Should().Be("merchant123");
        savedTransaction.Amount.Should().Be(100m);
        savedTransaction.Currency.Should().Be("BRL");
        savedTransaction.Direction.Should().Be(TransactionDirection.Credit);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction()
    {
        // Arrange
        var transaction = new Transaction(
            "merchant456",
            50m,
            "USD",
            TransactionDirection.Debit,
            DateOnly.FromDateTime(DateTime.UtcNow)
        );
        await _repository.AddAsync(transaction);

        // Act
        var result = await _repository.GetByIdAsync(transaction.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(transaction.Id);
        result.MerchantId.Should().Be("merchant456");
        result.Direction.Should().Be(TransactionDirection.Debit);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_MultipleTransactions_ShouldPersistAll()
    {
        // Arrange
        var transaction1 = new Transaction("merchant1", 100m, "BRL", TransactionDirection.Credit, DateOnly.FromDateTime(DateTime.UtcNow));
        var transaction2 = new Transaction("merchant2", 200m, "USD", TransactionDirection.Debit, DateOnly.FromDateTime(DateTime.UtcNow));

        // Act
        await _repository.AddAsync(transaction1);
        await _repository.AddAsync(transaction2);

        // Assert
        var count = await _context.Transactions.CountAsync();
        count.Should().Be(2);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}