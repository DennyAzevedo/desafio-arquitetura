using FluentAssertions;
using TransactionService.Domain.Entities;

namespace TransactionService.Tests.Unit;

public class OutboxTests
{
    [Fact]
    public void OutboxEvent_WhenCreated_ShouldHavePendingStatus()
    {
        // Arrange & Act
        var outboxEvent = new OutboxEvent(
            Guid.NewGuid(),
            "TransactionCreated",
            "{\"TransactionId\":\"123\"}"
        );

        // Assert
        outboxEvent.Status.Should().Be("Pending");
        outboxEvent.ProcessedOn.Should().BeNull();
        outboxEvent.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void OutboxEvent_MarkAsProcessed_ShouldUpdateStatusAndProcessedOn()
    {
        // Arrange
        var outboxEvent = new OutboxEvent(
            Guid.NewGuid(),
            "TransactionCreated",
            "{\"TransactionId\":\"123\"}"
        );

        // Act
        outboxEvent.MarkAsProcessed();

        // Assert
        outboxEvent.Status.Should().Be("Processed");
        outboxEvent.ProcessedOn.Should().NotBeNull();
        outboxEvent.ProcessedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void OutboxEvent_WithTransactionCreatedType_ShouldHaveCorrectType()
    {
        // Arrange & Act
        var outboxEvent = new OutboxEvent(
            Guid.NewGuid(),
            "TransactionCreated",
            "{\"TransactionId\":\"456\",\"Amount\":100}"
        );

        // Assert
        outboxEvent.Type.Should().Be("TransactionCreated");
        outboxEvent.Payload.Should().Contain("TransactionId");
        outboxEvent.Payload.Should().Contain("Amount");
    }

    [Fact]
    public void OutboxEvent_ShouldHaveUniqueId()
    {
        // Arrange & Act
        var event1 = new OutboxEvent(Guid.NewGuid(), "Type1", "Payload1");
        var event2 = new OutboxEvent(Guid.NewGuid(), "Type2", "Payload2");

        // Assert
        event1.Id.Should().NotBe(event2.Id);
        event1.Id.Should().NotBeEmpty();
        event2.Id.Should().NotBeEmpty();
    }
}