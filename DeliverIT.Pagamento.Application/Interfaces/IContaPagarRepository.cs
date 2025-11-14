using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeliverIT.Pagamento.Domain.Entities;

namespace DeliverIT.Pagamento.Application.Interfaces
{
    public interface IContaPagarRepository
    {
        Task AdicionarAsync(ContaPagar conta);
        Task<IEnumerable<ContaPagar>> ListarAsync();
    }
}
