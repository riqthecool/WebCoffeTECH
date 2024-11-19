using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class PedidoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://192.168.15.13:5257/api/Pedidos"; // URL da API de pedidos

        public PedidoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // Exibe a tela de pagamento (GET)
        [HttpGet]
        public IActionResult Pagamento()
        {
            return View(new PedidoViewModel());
        }

        // Processa o formulário de pagamento e cria um pedido (POST)
        [HttpPost]
        public async Task<IActionResult> Pagamento(PedidoViewModel pedido)
        {
            var contaId = HttpContext.Session.GetInt32("ContaId"); // Recuperar ContaId da sessão
            if (contaId == null)
            {
                ViewBag.ErrorMessage = "Você precisa estar logado para realizar um pedido.";
                return RedirectToAction("Login", "Auth");
            }

            // Associar o pedido ao usuário logado
            pedido.ContaId = contaId.Value;

            // Preencher dados padrão no pedido
            pedido.StatusPedido = "Pendente"; // Status inicial
            pedido.DataCriacao = DateTime.UtcNow; // Data atual para criação

            // Serializar o pedido em JSON
            var jsonContent = JsonSerializer.Serialize(pedido);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Enviar o pedido para o endpoint da API
            var response = await _httpClient.PostAsync(_apiBaseUrl, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Erro ao criar o pedido. Tente novamente mais tarde.";
                return View("Error");
            }

            // Redirecionar para Meus Pedidos após sucesso
            return RedirectToAction("MeusPedidos");
        }
    }
}
