using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DeliverIT.Pagamento.Domain.Entities;
using DeliverIT.Pagamento.Domain.Interfaces;
using DeliverIT.Pagamento.Application.Services;
using DeliverIT.Pagamento.Domain.Enums;
using DeliverIT.Pagamento.Application.DTOs;
using DeliverIT.Pagamento.Application.Interfaces;
 

namespace DeliverIT.Pagamento.Tests.Unit.Application
{
    public class ContaPagarServiceTests
    {
        private readonly Mock<IContaPagarRepository> _mockRepository;
        private readonly IContaPagarService _service;

        public ContaPagarServiceTests()
        {
            _mockRepository = new Mock<IContaPagarRepository>(); 
            _service = new ContaPagarService(_mockRepository.Object);
        }
         

        [Fact(DisplayName = "Service: Inclusão com Sucesso")]
        public async Task IncluirContaAsync_ComDadosValidos_DeveChamarAdicionarUmaVez()
        { 
            var inclusaoDto = new ContaPagarCadastroDTO
            {
                Nome = "Conta Teste",
                ValorOriginal = 100.00m,
                DataVencimento = DateTime.Today.AddDays(-1),
                DataPagamento = DateTime.Today
            };
             
            await _service.IncluirContaAsync(inclusaoDto);
             
            _mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<ContaPagar>()), Times.Once);
        }

        [Fact(DisplayName = "Service: Falha de Validação (Data Futura)")]
        public async Task IncluirContaAsync_ComDataFutura_DevePropagarArgumentExceptionDoDomain()
        {
            var inclusaoDto = new ContaPagarCadastroDTO
            {
                Nome = "Conta Futura",
                ValorOriginal = 100.00m,
                DataVencimento = DateTime.Today,
                DataPagamento = DateTime.Today.AddDays(1)
            };

            // Garante que o retorno seja uma Exception
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.IncluirContaAsync(inclusaoDto)
            );
        }


        [Fact(DisplayName = "Service: Listagem e Mapeamento Correto")]
        public async Task ListarContasAsync_ComContasNoRepo_DeveMapearParaListaDTO()
        {
            var contaEmDia = ContaPagar.Criar("Em Dia", 50.00m, DateTime.Today, DateTime.Today);

            var contaAtrasada = ContaPagar.Criar("Atrasada", 100.00m, DateTime.Today.AddDays(-5), DateTime.Today);

            var listaContas = new List<ContaPagar> { contaEmDia, contaAtrasada };

            _mockRepository.Setup(repo => repo.ListarAsync())
                .ReturnsAsync(listaContas);

            var resultado = (await _service.ListarContasAsync()).ToList();

            _mockRepository.Verify(repo => repo.ListarAsync(), Times.Once);

            // Garante que o objeto retornado pelo Servico e uma colecao
            Assert.IsAssignableFrom<IEnumerable<ContaPagarListaDTO>>(resultado);

            // Confirma a quantidade de items na lista
            Assert.Equal(2, resultado.Count);

            var contaAtrasadaMapeada = resultado.FirstOrDefault(c => c.DiasEmAtraso > 0);

            Assert.NotNull(contaAtrasadaMapeada);
            Assert.Equal(5, contaAtrasadaMapeada.DiasEmAtraso);
            Assert.Equal(104.00m, contaAtrasadaMapeada.ValorCorrigido);
        }
    }
}