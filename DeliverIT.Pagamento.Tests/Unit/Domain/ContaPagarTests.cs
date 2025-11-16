using System;
using Xunit;
using DeliverIT.Pagamento.Domain.Entities;
using DeliverIT.Pagamento.Domain.Enums;


namespace DeliverIT.Pagamento.Tests.Unit.Domain
{
    public class ContaPagarTests
    {
        private const decimal ValorBase = 100.00m;


        [Fact(DisplayName = "Cenário 1: Pagamento no Prazo (Zero Atraso)")]
        public void Criar_ContaPagaNoPrazo_DeveTerAtrasoZeroEValorOriginal()
        {
            var dataVencimento = DateTime.Today;
            var dataPagamento = DateTime.Today;

            var conta = ContaPagar.Criar("Conta Em Dia", ValorBase, dataVencimento, dataPagamento);

            Assert.Equal(0, conta.DiasEmAtraso);
            Assert.Equal(ValorBase, conta.ValorCorrigido);
            Assert.Equal(TipoRegraAtraso.EmDia, conta.RegraAplicada);
        }

        [Fact(DisplayName = "Cenário 2: Atraso de 1 Dia (Regra: Até 3 dias)")]
        public void Criar_ContaComAtrasoDeUmDia_DeveAplicarRegraAte3Dias()
        {
            var dataVencimento = DateTime.Today.AddDays(-1);
            var dataPagamento = DateTime.Today;

            var conta = ContaPagar.Criar("Conta 1 Dia Atraso", ValorBase, dataVencimento, dataPagamento);

            Assert.Equal(1, conta.DiasEmAtraso);
            Assert.Equal(102.10m, conta.ValorCorrigido);
            Assert.Equal(TipoRegraAtraso.Ate3Dias, conta.RegraAplicada);
        }

        [Fact(DisplayName = "Cenário 5: Atraso de 5 Dias (Limite Regra: Superior a 3 dias)")]
        public void Criar_ContaComAtrasoDeCincoDias_DeveAplicarRegraSuperiorA3Dias()
        {
            var dataVencimento = DateTime.Today.AddDays(-5);
            var dataPagamento = DateTime.Today;

            var conta = ContaPagar.Criar("Conta 5 Dias Atraso", ValorBase, dataVencimento, dataPagamento);

            Assert.Equal(5, conta.DiasEmAtraso);
            Assert.Equal(104.00m, conta.ValorCorrigido);
            Assert.Equal(TipoRegraAtraso.SuperiorA3Dias, conta.RegraAplicada);
        }

        [Fact(DisplayName = "Cenário 6: Atraso de 6 Dias (Regra: Superior a 5 dias)")]
        public void Criar_ContaComAtrasoDeSeisDias_DeveAplicarRegraSuperiorA5Dias()
        {
            var dataVencimento = DateTime.Today.AddDays(-6);
            var dataPagamento = DateTime.Today;

            var conta = ContaPagar.Criar("Conta 6 Dias Atraso", ValorBase, dataVencimento, dataPagamento);

            Assert.Equal(6, conta.DiasEmAtraso);
            Assert.Equal(106.80m, conta.ValorCorrigido);
            Assert.Equal(TipoRegraAtraso.SuperiorA5Dias, conta.RegraAplicada);
        }


        [Fact(DisplayName = "Cenário 9: Valor Original Inválido (Zero)")]
        public void Criar_ContaComValorZero_DeveLancarArgumentException()
        {
            var valorInvalido = 0.00m;

            var ex = Assert.Throws<ArgumentException>(() =>
                ContaPagar.Criar(
                    "Conta Teste",
                    valorInvalido,
                    DateTime.Today,
                    DateTime.Today
                )
            );
            Assert.Contains("Valor Original deve ser maior que zero.", ex.Message);
        }

        [Fact(DisplayName = "Cenário 7/9: Data de Pagamento Futura (Validação Adicionada)")]
        public void Criar_ContaComDataPagamentoFutura_DeveLancarArgumentException()
        {
            var dataPagamentoFutura = DateTime.Today.AddDays(1); 

            var ex = Assert.Throws<ArgumentException>(() =>
                ContaPagar.Criar(
                    "Conta Futura (Falha)",
                    ValorBase,
                    DateTime.Today,
                    dataPagamentoFutura
                )
            );

            Assert.Contains("A data de pagamento não pode ser uma data futura.", ex.Message);
        }
    }
}