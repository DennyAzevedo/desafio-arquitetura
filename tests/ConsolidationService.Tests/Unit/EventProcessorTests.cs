using ConsolidationService.Application.Services;
using ConsolidationService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ConsolidationService.Tests.Unit;

public class ConsolidationQueryServiceTests
{
    private readonly Mock<IDailyBalanceRepository> _repositoryMock;
    private readonly ConsolidationQueryService _service;

    public ConsolidationQueryServiceTests()
    {
        _repositoryMock = new Mock<IDailyBalanceRepository>();
        _service = new ConsolidationQueryService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetDailyBalanceAsync_WithValidData_ShouldReturnDto()
    {
        var merchantId = "merchant123";
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var dailyBalance = new DailyBalance(merchantId, date, 100m, 30m);

        _repositoryMock.Setup(x => x.GetDailyBalanceAsync(merchantId, date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dailyBalance);

        var query = new ConsolidationService.Application.Queries.GetDailyBalanceQuery(merchantId, date);
        var result = await _service.GetDailyBalanceAsync(query);

        result.Should().NotBeNull();
        result!.MerchantId.Should().Be(merchantId);
        result.TotalCredit.Should().Be(100m);
        result.TotalDebit.Should().Be(30m);
        result.Balance.Should().Be(70m);
    }

    [Fact]
    public async Task GetDailyBalanceAsync_WhenNoData_ShouldReturnNull()
    {
        var merchantId = "merchant123";
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        _repositoryMock.Setup(x => x.GetDailyBalanceAsync(merchantId, date, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DailyBalance?)null);

        var query = new ConsolidationService.Application.Queries.GetDailyBalanceQuery(merchantId, date);
        var result = await _service.GetDailyBalanceAsync(query);

        result.Should().BeNull();
    }
}