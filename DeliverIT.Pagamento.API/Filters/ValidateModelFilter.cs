using DeliverIT.Pagamento.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeliverIT.Pagamento.API.Filters
{
    public class ValidateModelFilter : IActionFilter
    {
        // OnActionExecuting execute ANTES do metodo do Controller
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.ToValidacaoErro();

                context.Result = new BadRequestObjectResult(errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nenhuma lógica necessária, pois o OnActionExecuting já esta tratando o erro
        }
    }
}