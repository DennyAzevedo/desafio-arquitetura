using FluentAssertions;
using Moq;
using TransactionService.Application.Commands;
using TransactionService.Application.Handlers;
using TransactionService.Application.Services;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;

namespace TransactionService.Tests.Unit;

public class CreateTransactionTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IOutboxRepository> _outboxRepositoryMock;
    private readonly TransactionApplicationService _service;

    public CreateTransactionTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _outboxRepositoryMock = new Mock<IOutboxRepository>();
        _service = new TransactionApplicationService(_transactionRepositoryMock.Object, _outboxRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithValidCommand_ShouldCreateTransactionAndOutboxEvent()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.MerchantId.Should().Be(command.MerchantId);
        result.Amount.Should().Be(command.Amount);
        result.Currency.Should().Be(command.Currency);
        result.Direction.Should().Be(command.Direction);

        _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once);
        _outboxRepositoryMock.Verify(x => x.AddAsync(It.IsAny<OutboxEvent>()), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithDebitDirection_ShouldCreateCorrectTransaction()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            50m,
            "USD",
            TransactionDirection.Debit,
            DateTime.UtcNow
        );

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.Direction.Should().Be(TransactionDirection.Debit);
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.Is<Transaction>(
            t => t.Direction == TransactionDirection.Debit && t.Amount == 50m
        )), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_ShouldCreateOutboxEventWithCorrectType()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            Guid.NewGuid(),
            200m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Act
        await _service.CreateTransactionAsync(command);

        // Assert
        _outboxRepositoryMock.Verify(x => x.AddAsync(It.Is<OutboxEvent>(
            e => e.Type == "TransactionCreated"
        )), Times.Once);
    }
}