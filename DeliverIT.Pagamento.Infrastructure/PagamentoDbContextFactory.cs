using DeliverIT.Pagamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace DeliverIT.Pagamento.Infrastructure
{
    public class PagamentoDbContextFactory : IDesignTimeDbContextFactory<PagamentoDbContext>
    {
        public PagamentoDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false)
              .Build();

            var builder = new DbContextOptionsBuilder<PagamentoDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
             
            builder.UseNpgsql(connectionString);

            return new PagamentoDbContext(builder.Options);
        }
    }
}