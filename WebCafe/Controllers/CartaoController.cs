using Microsoft.AspNetCore.Mvc;
using WebCafe.Models;
using System.Threading.Tasks;

namespace WebCafe.Controllers
{
    public class CartaoController : Controller
    {
        [HttpGet]
        public IActionResult NovoCartao(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            // Especifica explicitamente o caminho da view
            return View("~/Views/Home/NovoCartao.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> NovoCartao(Cartao model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Simula o salvamento no banco de dados
                await Task.CompletedTask;

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl); // Redireciona de volta para pagamento
                }

                return RedirectToAction("MeusCartoes");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Home/NovoCartao.cshtml", model); // Caminho explícito
        }
    }
}
