using FluentAssertions;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Enums;

namespace TransactionService.Tests.Unit;

public class ValidationTests
{
    [Fact]
    public void Transaction_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange & Act
        var transaction = new Transaction(
            Guid.NewGuid(),
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Assert
        transaction.Should().NotBeNull();
        transaction.Id.Should().NotBeEmpty();
        transaction.Amount.Should().Be(100m);
        transaction.Currency.Should().Be("BRL");
        transaction.Direction.Should().Be(TransactionDirection.Credit);
    }

    [Fact]
    public void Transaction_WithCreditDirection_ShouldHaveCorrectDirection()
    {
        // Arrange & Act
        var transaction = new Transaction(
            Guid.NewGuid(),
            50m,
            "USD",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Assert
        transaction.Direction.Should().Be(TransactionDirection.Credit);
    }

    [Fact]
    public void Transaction_WithDebitDirection_ShouldHaveCorrectDirection()
    {
        // Arrange & Act
        var transaction = new Transaction(
            Guid.NewGuid(),
            75m,
            "EUR",
            TransactionDirection.Debit,
            DateTime.UtcNow
        );

        // Assert
        transaction.Direction.Should().Be(TransactionDirection.Debit);
    }

    [Theory]
    [InlineData("BRL")]
    [InlineData("USD")]
    [InlineData("EUR")]
    public void Transaction_WithValidCurrency_ShouldAcceptCurrency(string currency)
    {
        // Arrange & Act
        var transaction = new Transaction(
            Guid.NewGuid(),
            100m,
            currency,
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Assert
        transaction.Currency.Should().Be(currency);
    }
}