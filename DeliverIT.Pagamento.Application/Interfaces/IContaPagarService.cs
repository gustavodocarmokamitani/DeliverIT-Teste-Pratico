using System.Collections.Generic;
using System.Threading.Tasks;
using DeliverIT.Pagamento.Application.DTOs;

namespace DeliverIT.Pagamento.Application.Interfaces
{
    public interface IContaPagarService
    {
        // O Servico realiza a transformacao DTO -> Entidade 
        // e chama o metodo do Domain para criar a entidade e aplicar a regras de negocios.  
        Task IncluirContaAsync(ContaPagarCadastroDTO dto);

        Task<IEnumerable<ContaPagarListaDTO>> ListarContasAsync();
    }
}
