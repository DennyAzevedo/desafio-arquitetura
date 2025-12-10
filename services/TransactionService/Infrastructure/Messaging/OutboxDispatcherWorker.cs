using TransactionService.Application.Services;

namespace TransactionService.Infrastructure.Messaging;

public class OutboxDispatcherWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxDispatcherWorker> _logger;

    public OutboxDispatcherWorker(IServiceProvider serviceProvider, ILogger<OutboxDispatcherWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OutboxDispatcherWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

                var pendingEvents = await outboxRepository.GetPendingEventsAsync();

                foreach (var @event in pendingEvents)
                {
                    try
                    {
                        publisher.Publish(@event.Payload);
                        @event.MarkAsProcessed();
                        await outboxRepository.UpdateAsync(@event);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error publishing event {EventId}", @event.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OutboxDispatcherWorker");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
