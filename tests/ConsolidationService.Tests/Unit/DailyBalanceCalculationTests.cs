using ConsolidationService.Domain.Entities;
using FluentAssertions;

namespace ConsolidationService.Tests.Unit;

public class DailyBalanceCalculationTests
{
    [Fact]
    public void DailyBalance_WhenCreated_ShouldHaveZeroBalance()
    {
        var merchantId = Guid.NewGuid();
        var date = DateTime.UtcNow.Date;

        var dailyBalance = new DailyBalance(merchantId, date);

        dailyBalance.TotalCredit.Should().Be(0);
        dailyBalance.TotalDebit.Should().Be(0);
        dailyBalance.Balance.Should().Be(0);
    }

    [Fact]
    public void AddCredit_ShouldIncreaseBalance()
    {
        var dailyBalance = new DailyBalance(Guid.NewGuid(), DateTime.UtcNow.Date);

        dailyBalance.AddCredit(100m);

        dailyBalance.TotalCredit.Should().Be(100m);
        dailyBalance.Balance.Should().Be(100m);
    }

    [Fact]
    public void AddDebit_ShouldDecreaseBalance()
    {
        var dailyBalance = new DailyBalance(Guid.NewGuid(), DateTime.UtcNow.Date);

        dailyBalance.AddDebit(50m);

        dailyBalance.TotalDebit.Should().Be(50m);
        dailyBalance.Balance.Should().Be(-50m);
    }

    [Fact]
    public void Balance_ShouldBeCalculatedCorrectly()
    {
        var dailyBalance = new DailyBalance(Guid.NewGuid(), DateTime.UtcNow.Date);

        dailyBalance.AddCredit(200m);
        dailyBalance.AddDebit(75m);
        dailyBalance.AddCredit(50m);

        dailyBalance.TotalCredit.Should().Be(250m);
        dailyBalance.TotalDebit.Should().Be(75m);
        dailyBalance.Balance.Should().Be(175m);
    }

    [Fact]
    public void MultipleCreditsAndDebits_ShouldAccumulateCorrectly()
    {
        var dailyBalance = new DailyBalance(Guid.NewGuid(), DateTime.UtcNow.Date);

        dailyBalance.AddCredit(100m);
        dailyBalance.AddCredit(150m);
        dailyBalance.AddDebit(30m);
        dailyBalance.AddDebit(20m);

        dailyBalance.TotalCredit.Should().Be(250m);
        dailyBalance.TotalDebit.Should().Be(50m);
        dailyBalance.Balance.Should().Be(200m);
    }
}
