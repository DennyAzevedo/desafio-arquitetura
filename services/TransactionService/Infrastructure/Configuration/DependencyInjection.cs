using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Handlers;
using TransactionService.Application.Services;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;

namespace TransactionService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CashFlowDb")));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<TransactionApplicationService>();

        return services;
    }
}
