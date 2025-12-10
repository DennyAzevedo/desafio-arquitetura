using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Handlers;
using TransactionService.Application.Services;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;

namespace TransactionService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("TransactionDatabase")));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<TransactionApplicationService>();
        services.AddScoped<RabbitMqPublisher>();

        services.AddHostedService<OutboxDispatcherWorker>();

        return services;
    }
}
