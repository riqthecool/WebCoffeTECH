using Microsoft.AspNetCore.Mvc;
using WebCafe.Models;
using System.Threading.Tasks;

namespace WebCafe.Controllers
{
    public class EnderecoController : Controller
    {
        [HttpGet]
        public IActionResult NovoEndereco(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            // Especifica explicitamente o caminho da view
            return View("~/Views/Home/NovoEndereco.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> NovoEndereco(Endereco model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Simula o salvamento no banco de dados
                await Task.CompletedTask;

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl); // Redireciona de volta para pagamento
                }

                return RedirectToAction("MeusEnderecos");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Home/NovoEndereco.cshtml", model); // Caminho explícito
        }
    }
}
