using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;

namespace TransactionService.Tests.Integration;

[Trait("Category", "Integration")]
public class OutboxDispatcherTests : IDisposable
{
    private readonly TransactionDbContext _context;
    private readonly OutboxRepository _outboxRepository;
    private readonly Mock<RabbitMqPublisher> _publisherMock;
    private readonly ServiceProvider _serviceProvider;

    public OutboxDispatcherTests()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TransactionDbContext(options);
        _outboxRepository = new OutboxRepository(_context);
        _publisherMock = new Mock<RabbitMqPublisher>();

        var services = new ServiceCollection();
        services.AddSingleton(_outboxRepository);
        services.AddSingleton(_publisherMock.Object);
        services.AddSingleton<Mock<ILogger<OutboxDispatcherWorker>>>(new Mock<ILogger<OutboxDispatcherWorker>>());
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task GetPendingEventsAsync_ShouldReturnPendingEvents()
    {
        // Arrange
        var event1 = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", "{\"id\":1}");
        var event2 = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", "{\"id\":2}");
        
        await _outboxRepository.AddAsync(event1);
        await _outboxRepository.AddAsync(event2);

        // Act
        var pendingEvents = await _outboxRepository.GetPendingEventsAsync();

        // Assert
        pendingEvents.Should().HaveCount(2);
        pendingEvents.Should().AllSatisfy(e => e.Status.Should().Be("Pending"));
    }

    [Fact]
    public async Task UpdateAsync_ShouldMarkEventAsProcessed()
    {
        // Arrange
        var outboxEvent = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", "{\"id\":1}");
        await _outboxRepository.AddAsync(outboxEvent);

        // Act
        outboxEvent.MarkAsProcessed();
        await _outboxRepository.UpdateAsync(outboxEvent);

        // Assert
        var updatedEvent = await _context.OutboxEvents.FirstOrDefaultAsync(e => e.Id == outboxEvent.Id);
        updatedEvent.Should().NotBeNull();
        updatedEvent!.Status.Should().Be("Processed");
        updatedEvent.ProcessedOn.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPendingEventsAsync_ShouldNotReturnProcessedEvents()
    {
        // Arrange
        var pendingEvent = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", "{\"id\":1}");
        var processedEvent = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", "{\"id\":2}");
        
        await _outboxRepository.AddAsync(pendingEvent);
        await _outboxRepository.AddAsync(processedEvent);
        
        processedEvent.MarkAsProcessed();
        await _outboxRepository.UpdateAsync(processedEvent);

        // Act
        var pendingEvents = await _outboxRepository.GetPendingEventsAsync();

        // Assert
        pendingEvents.Should().HaveCount(1);
        pendingEvents.First().Id.Should().Be(pendingEvent.Id);
    }

    [Fact]
    public async Task GetPendingEventsAsync_ShouldLimitResults()
    {
        // Arrange
        for (int i = 0; i < 15; i++)
        {
            var outboxEvent = new OutboxEvent(Guid.NewGuid(), "TransactionCreated", $"{{\"id\":{i}}}");
            await _outboxRepository.AddAsync(outboxEvent);
        }

        // Act
        var pendingEvents = await _outboxRepository.GetPendingEventsAsync();

        // Assert
        pendingEvents.Should().HaveCount(10); // Repository limits to 10
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _serviceProvider.Dispose();
    }
}