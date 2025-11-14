using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using DeliverIT.Pagamento.API.Models; 

namespace DeliverIT.Pagamento.API.Extensions
{ 
    public static class ValidacaoExtensao
    {
        // Converte o ModelState (erros do ASP.NET Core) em uma lista limpa e padronizada de ValidationError.
        public static IEnumerable<ValidacaoErro> ToValidacaoErro(this ModelStateDictionary modelState)
        {
            var errors = modelState.Where(m => m.Value.Errors.Any())
                .SelectMany(m => m.Value.Errors.Select(e => new ValidacaoErro
                { 
                    Field = m.Key, 
                    Message = e.ErrorMessage
                }))
                .ToList();

            return errors;
        }
    }
}