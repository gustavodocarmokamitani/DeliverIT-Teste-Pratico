using DeliverIT.Pagamento.API.Extensions;
using DeliverIT.Pagamento.Application.DTOs;
using DeliverIT.Pagamento.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliverIT.Pagamento.API.Controllers
{
    [ApiController]
    [Route("api/v1/contas-pagar")]
    public class ContasPagarController : ControllerBase
    {
        private readonly IContaPagarService _contaPagarService;

        public ContasPagarController(IContaPagarService contaPagarService)
        {
            _contaPagarService = contaPagarService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContaPagarListaDTO>), 200)]
        public async Task<IActionResult> ListarContas()
        {
            var contas = await _contaPagarService.ListarContasAsync();
            return Ok(contas);
        }

        [HttpPost]
        [ProducesResponseType(201)] //Criado com sucesso
        [ProducesResponseType(400)] //Falha na requisicao 
        public async Task<IActionResult> IncluirConta([FromBody] ContaPagarCadastroDTO dto)
        {
            // Caso a validação falhe. O ValidateModelFilter que está registrado globalmente já intercepta a requisicao e retorna o erro.
            // ( Removendo a redundancia do codigo )
            // Nesse caso como o projeto é pequeno, nao a necessidade, mas gostaria de adicionar o principio DRY (Don't Repeat Yourself)
            await _contaPagarService.IncluirContaAsync(dto);
            return StatusCode(201);
        }

    }
}
