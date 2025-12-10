using ConsolidationService.Application.Services;
using ConsolidationService.Infrastructure.Persistence;
using ConsolidationService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConsolidationService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReadDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CashFlowDb")));

        services.AddScoped<IDailyBalanceRepository, DailyBalanceRepository>();
        services.AddScoped<ConsolidationQueryService>();

        return services;
    }
}
