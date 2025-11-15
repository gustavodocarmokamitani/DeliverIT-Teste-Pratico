using DeliverIT.Pagamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliverIT.Pagamento.Infrastructure.Data
{
    public class PagamentoDbContext : DbContext
    {
        public DbSet<ContaPagar> ContasPagar {  get; set; }

        public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaPagar>().HasKey(c => c.Id);
            modelBuilder.Entity<ContaPagar>().Property(c => c.Nome).IsRequired();
            modelBuilder.Entity<ContaPagar>().Property(c => c.ValorOriginal).IsRequired();
            modelBuilder.Entity<ContaPagar>().Property(c => c.ValorCorrigido).IsRequired();
            modelBuilder.Entity<ContaPagar>()
                .Property(c => c.RegraAplicada)
                .HasConversion<int>();
        }
    }
}
