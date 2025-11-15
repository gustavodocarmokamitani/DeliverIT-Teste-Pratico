using System.Collections.Generic; 
using System.Threading.Tasks;
using DeliverIT.Pagamento.Application.DTOs;
using DeliverIT.Pagamento.Application.Interfaces;
using DeliverIT.Pagamento.Domain.Interfaces;
using DeliverIT.Pagamento.Domain.Entities;
using System.Linq;

namespace DeliverIT.Pagamento.Application.Services
{
    public class ContaPagarService : IContaPagarService
    {
        private readonly IContaPagarRepository _repository;

        // Injecao de Dependencia.
        public ContaPagarService(IContaPagarRepository repository)
        {
            _repository = repository;
        }

        //Caso de uso para adicionar uma conta
        public async Task IncluirContaAsync(ContaPagarCadastroDTO dto)
        {
            var conta = ContaPagar.Criar(
                dto.Nome,
                dto.ValorOriginal,
                dto.DataVencimento,
                dto.DataPagamento
                );

            await _repository.AdicionarAsync(conta);
        }

        //Caso de uso para listar as contas
        public async Task<IEnumerable<ContaPagarListaDTO>> ListarContasAsync()
        {
            var contas = await _repository.ListarAsync();

            var listaDTO = contas.Select(c => new ContaPagarListaDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                ValorOriginal = c.ValorOriginal,
                ValorCorrigido = c.ValorCorrigido,
                DiasEmAtraso = c.DiasEmAtraso,
                DataVencimento = c.DataVencimento,
                DataPagamento = c.DataPagamento,
                RegraAplicada = c.RegraAplicada.ToString(),
            }).ToList();

            return listaDTO;
        }
    }
}
