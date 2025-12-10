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
    private readonly TransactionApplicationService _service;

    public CreateTransactionTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _service = new TransactionApplicationService(_transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithValidCommand_ShouldCreateTransaction()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            "merchant123",
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.Should().NotBeEmpty();
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.Is<Transaction>(
            t => t.MerchantId == command.MerchantId && 
                 t.Amount == command.Amount && 
                 t.Currency == command.Currency && 
                 t.Direction == command.Direction
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithDebitDirection_ShouldCreateCorrectTransaction()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            "merchant456",
            50m,
            "USD",
            TransactionDirection.Debit,
            DateTime.UtcNow
        );

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.Should().NotBeEmpty();
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.Is<Transaction>(
            t => t.Direction == TransactionDirection.Debit && t.Amount == 50m
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_WithValidData_ShouldReturnTransactionId()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            "merchant789",
            200m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.Should().NotBeEmpty();
        _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}