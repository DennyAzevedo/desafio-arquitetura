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
            "merchant123",
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Assert
        transaction.Should().NotBeNull();
        transaction.Id.Should().NotBeEmpty();
        transaction.MerchantId.Should().Be("merchant123");
        transaction.Amount.Should().Be(100m);
        transaction.Currency.Should().Be("BRL");
        transaction.Direction.Should().Be(TransactionDirection.Credit);
        transaction.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Transaction_WithZeroAmount_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Transaction(
            "merchant123",
            0m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        act.Should().Throw<ArgumentException>().WithMessage("Amount must be greater than zero*");
    }

    [Fact]
    public void Transaction_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Transaction(
            "merchant123",
            -10m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        act.Should().Throw<ArgumentException>().WithMessage("Amount must be greater than zero*");
    }

    [Fact]
    public void Transaction_WithEmptyMerchantId_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Transaction(
            "",
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        act.Should().Throw<ArgumentException>().WithMessage("MerchantId is required*");
    }

    [Fact]
    public void Transaction_IsCredit_ShouldReturnTrueForCreditDirection()
    {
        // Arrange
        var transaction = new Transaction(
            "merchant123",
            100m,
            "BRL",
            TransactionDirection.Credit,
            DateTime.UtcNow
        );

        // Act & Assert
        transaction.IsCredit().Should().BeTrue();
        transaction.IsDebit().Should().BeFalse();
    }

    [Fact]
    public void Transaction_IsDebit_ShouldReturnTrueForDebitDirection()
    {
        // Arrange
        var transaction = new Transaction(
            "merchant123",
            100m,
            "BRL",
            TransactionDirection.Debit,
            DateTime.UtcNow
        );

        // Act & Assert
        transaction.IsDebit().Should().BeTrue();
        transaction.IsCredit().Should().BeFalse();
    }
}