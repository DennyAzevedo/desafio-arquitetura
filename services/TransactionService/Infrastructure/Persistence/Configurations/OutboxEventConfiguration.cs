using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.ToTable("outbox_events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AggregateId).IsRequired();
        builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Payload).IsRequired();
        builder.Property(x => x.OccurredOn).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();

        builder.HasIndex(x => x.Status);
    }
}
