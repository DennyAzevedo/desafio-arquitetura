using Microsoft.EntityFrameworkCore;

namespace ConsolidationService.Infrastructure.Persistence;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração para acessar tabela transactions do TransactionService
        modelBuilder.Entity<TransactionEntity>(entity =>
        {
            entity.ToTable("transactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MerchantId).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Direction).IsRequired();
            entity.Property(e => e.OccurredAt).IsRequired();
        });
    }

    // Entidade para mapear a tabela transactions
    public class TransactionEntity
    {
        public Guid Id { get; set; }
        public string MerchantId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int Direction { get; set; } // 0 = Credit, 1 = Debit
        public DateTime OccurredAt { get; set; }
    }
}
