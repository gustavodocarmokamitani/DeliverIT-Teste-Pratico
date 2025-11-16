using System;
using System.Collections.Generic;
using System.Text;

namespace DeliverIT.Pagamento.Application.DTOs
{
    public class ContaPagarListaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorCorrigido { get; set; }
        public int DiasEmAtraso { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
