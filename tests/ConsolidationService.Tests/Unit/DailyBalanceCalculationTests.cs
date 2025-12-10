using ConsolidationService.Domain.Entities;
using FluentAssertions;

namespace ConsolidationService.Tests.Unit;

public class DailyBalanceCalculationTests
{
    [Fact]
    public void DailyBalance_WhenCreated_ShouldCalculateBalanceCorrectly()
    {
        var merchantId = "merchant123";
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var totalCredit = 100m;
        var totalDebit = 30m;

        var dailyBalance = new DailyBalance(merchantId, date, totalCredit, totalDebit);

        dailyBalance.MerchantId.Should().Be(merchantId);
        dailyBalance.Date.Should().Be(date);
        dailyBalance.TotalCredit.Should().Be(totalCredit);
        dailyBalance.TotalDebit.Should().Be(totalDebit);
        dailyBalance.Balance.Should().Be(70m);
    }

    [Fact]
    public void DailyBalance_WithOnlyCredits_ShouldHavePositiveBalance()
    {
        var dailyBalance = new DailyBalance("merchant123", DateOnly.FromDateTime(DateTime.UtcNow), 200m, 0m);

        dailyBalance.Balance.Should().Be(200m);
    }

    [Fact]
    public void DailyBalance_WithOnlyDebits_ShouldHaveNegativeBalance()
    {
        var dailyBalance = new DailyBalance("merchant123", DateOnly.FromDateTime(DateTime.UtcNow), 0m, 150m);

        dailyBalance.Balance.Should().Be(-150m);
    }

    [Fact]
    public void DailyBalance_WithEqualCreditsAndDebits_ShouldHaveZeroBalance()
    {
        var dailyBalance = new DailyBalance("merchant123", DateOnly.FromDateTime(DateTime.UtcNow), 100m, 100m);

        dailyBalance.Balance.Should().Be(0m);
    }

    [Fact]
    public void DailyBalance_WithVariousAmounts_ShouldCalculateBalanceCorrectly()
    {
        var testCases = new[]
        {
            new { Credit = 500m, Debit = 200m, Expected = 300m },
            new { Credit = 100m, Debit = 150m, Expected = -50m },
            new { Credit = 0m, Debit = 0m, Expected = 0m }
        };

        foreach (var testCase in testCases)
        {
            var dailyBalance = new DailyBalance("merchant123", DateOnly.FromDateTime(DateTime.UtcNow), testCase.Credit, testCase.Debit);
            dailyBalance.Balance.Should().Be(testCase.Expected);
        }
    }
}
