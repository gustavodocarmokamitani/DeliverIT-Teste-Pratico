using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using DeliverIT.Pagamento.Domain.Entities;
using DeliverIT.Pagamento.Infrastructure.Data;
using DeliverIT.Pagamento.Infrastructure.Repositories;
using DeliverIT.Pagamento.Domain.Enums;

namespace DeliverIT.Pagamento.Tests.Integration.Infrastructure
{
    public class ContaPagarRepositoryTests : IDisposable
    {
        private readonly PagamentoDbContext _context;
        private readonly ContaPagarRepository _repository;

        public ContaPagarRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PagamentoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PagamentoDbContext(options);
            _repository = new ContaPagarRepository(_context);
        }


        [Fact(DisplayName = "Repository: AdicionarAsync deve persistir a Entidade")]
        public async Task AdicionarAsync_DevePersistirContaNoBanco()
        {
            var conta = ContaPagar.Criar("Conta Teste Repositório", 150.00m, DateTime.Today.AddDays(-2), DateTime.Today);

            await _repository.AdicionarAsync(conta);
            await _context.SaveChangesAsync();

            var contaPersistida = await _context.ContasPagar.FirstOrDefaultAsync(c => c.Nome == "Conta Teste Repositório");

            Assert.NotNull(contaPersistida);
            Assert.Equal(2, contaPersistida.DiasEmAtraso);
            Assert.Equal(TipoRegraAtraso.Ate3Dias, contaPersistida.RegraAplicada);
        }

        [Fact(DisplayName = "Repository: ListarAsync deve retornar todas as Contas")]
        public async Task ListarAsync_DeveRetornarTodasAsContasPersistidas()
        {
            var conta1 = ContaPagar.Criar("Conta 1", 50.00m, DateTime.Today, DateTime.Today);
            var conta2 = ContaPagar.Criar("Conta 2", 100.00m, DateTime.Today.AddDays(-10), DateTime.Today);

            await _repository.AdicionarAsync(conta1);
            await _repository.AdicionarAsync(conta2);
            await _context.SaveChangesAsync();

            var lista = await _repository.ListarAsync();

            Assert.Equal(2, lista.Count());

            var contaAtrasada = lista.FirstOrDefault(c => c.Nome == "Conta 2");
            Assert.NotNull(contaAtrasada);
            Assert.Equal(TipoRegraAtraso.SuperiorA5Dias, contaAtrasada.RegraAplicada);
        }


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}