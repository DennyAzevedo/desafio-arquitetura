using ConsolidationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsolidationService.Infrastructure.Persistence.Configurations;

public class DailyBalanceConfiguration : IEntityTypeConfiguration<DailyBalance>
{
    public void Configure(EntityTypeBuilder<DailyBalance> builder)
    {
        builder.ToTable("daily_balances");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MerchantId).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.TotalCredit).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TotalDebit).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Balance).HasColumnType("decimal(18,2)");

        builder.HasIndex(x => new { x.MerchantId, x.Date }).IsUnique();
    }
}
