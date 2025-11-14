using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeliverIT.Pagamento.Application.DTOs
{
    public class ContaPagarCadastroDTO
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Valor Original é obrigatório.")]
        public decimal ValorOriginal { get; set; }

        [Required(ErrorMessage = "A Data de Vencimento é obrigatória.")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "A Data de Pagamento é obrigatória.")]
        public DateTime DataPagamento { get; set; }
    }
}
