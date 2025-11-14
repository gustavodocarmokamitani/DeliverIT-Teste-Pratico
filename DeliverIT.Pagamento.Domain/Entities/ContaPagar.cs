using System;
using System.Collections.Generic;
using DeliverIT.Pagamento.Domain.Enums;
using System.Linq;

namespace DeliverIT.Pagamento.Domain.Entities
{
    public class ContaPagar
    {
        public int Id { get; set; }
        public string Nome { get; private set; }
        public decimal ValorOriginal { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime DataPagamento { get; private set; }

        public decimal ValorCorrigido { get; private set; }
        public int DiasEmAtraso { get; private set; }
        public TipoRegraAtraso RegraAplicada { get; private set; }

        // Construtor 
        private ContaPagar() { }

        // Metodo para criar a entidade aplicando as regras de negocio.
        public static ContaPagar Criar(string nome, decimal valorOriginal, DateTime dataVencimento, DateTime dataPagamento)
        {
            // Verificacao dos campos 
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(nome))
            {
                erros.Add("Nome é obrigatório.");
            }

            if (valorOriginal <= 0)
            {
                erros.Add("Valor Original deve ser maior que zero.");
            }

            if (dataVencimento == default)
            {
                erros.Add("Data de Vencimento é obrigatória.");
            }

            if (dataPagamento == default)
            {
                erros.Add("Data de Pagamento é obrigatória.");
            }

            if (erros.Any()) 
            {
                var mensagem = "Falha na criação da ContaPagar. Campos incorretos: " + string.Join(", ", erros);
                throw new ArgumentException(mensagem);
            }

            var conta = new ContaPagar
            {
                Nome = nome,
                ValorOriginal = valorOriginal,
                DataVencimento = dataVencimento,
                DataPagamento = dataPagamento
            };

            conta.AplicarRegraAtraso();

            return conta;
        }

        // Metodo que contem a logica que calcula e define o estado da entidade.
        public void AplicarRegraAtraso()
        {
            // Capturando a quantidade de dias atrasados
            var diasAtraso = (DataPagamento - DataVencimento).Days;

            if (diasAtraso <= 0)
            {
                this.DiasEmAtraso = 0;
                this.ValorCorrigido = this.ValorOriginal;
                this.RegraAplicada = TipoRegraAtraso.EmDia;
                return;
            }

            this.DiasEmAtraso = diasAtraso;
            decimal multaPercentual = 0;
            decimal jurosDiarioPercentual = 0;
            TipoRegraAtraso regra = TipoRegraAtraso.EmDia;

            //Definir Multa e Juros
            if (diasAtraso <= 3)
            {
                //Utilizando o sufixo M para que o interpretador represente como decimal
                multaPercentual = 0.02M;
                jurosDiarioPercentual = 0.001M;
                regra = TipoRegraAtraso.Ate3Dias;
            }
            else if (diasAtraso <= 5)
            {
                multaPercentual = 0.03M;
                jurosDiarioPercentual = 0.002M;
                regra = TipoRegraAtraso.SuperiorA3Dias;
            }
            else
            {
                multaPercentual = 0.05M;
                jurosDiarioPercentual = 0.003M;
                regra = TipoRegraAtraso.SuperiorA5Dias;
            }

            // Calculo do valor corrigido
            decimal valorMulta = ValorOriginal * multaPercentual;
            decimal valorJuros = ValorOriginal * jurosDiarioPercentual * DiasEmAtraso;

            this.ValorCorrigido = ValorOriginal + valorMulta + valorJuros;
            this.RegraAplicada = regra;
        }
    }
}
