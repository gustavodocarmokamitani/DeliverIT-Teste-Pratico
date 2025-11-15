using DeliverIT.Pagamento.Domain.Entities;
using DeliverIT.Pagamento.Domain.Interfaces;
using DeliverIT.Pagamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliverIT.Pagamento.Infrastructure.Repositories
{ 
    public class ContaPagarRepository : IContaPagarRepository
    {
        private readonly PagamentoDbContext _context;

        public ContaPagarRepository(PagamentoDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ContaPagar conta)
        {
            await _context.ContasPagar.AddAsync(conta); 
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContaPagar>> ListarAsync()
        { 
            return await _context.ContasPagar.AsNoTracking().ToListAsync();
        }
    }
}