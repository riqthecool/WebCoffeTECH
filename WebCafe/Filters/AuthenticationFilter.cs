using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebCafe.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verifica se o token está presente na sessão
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("Token")))
            {
                // Redireciona para a tela de login se não estiver autenticado
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
