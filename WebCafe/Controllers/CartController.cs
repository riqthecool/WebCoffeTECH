using Microsoft.AspNetCore.Mvc;
using WebCafe.Models;
using WebCafe.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebCafe.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://192.168.15.13:5257/api";

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // Adiciona um item ao carrinho
        [HttpGet]
        public IActionResult AddToCart(int id, string name, decimal price, string imageUrl, string variant = "default")
        {
            // Obtém o carrinho da sessão ou inicializa um novo
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            string uniqueId = $"{id}_{variant}";

            // Verifica se o item já está no carrinho
            var itemInCart = cart.FirstOrDefault(i => i.UniqueId == uniqueId);

            if (itemInCart == null)
            {
                // Adiciona o item ao carrinho se ainda não existir
                cart.Add(new CartItem
                {
                    UniqueId = uniqueId,
                    Id = id,
                    Name = name,
                    Price = price,
                    Quantity = 1,
                    ImageUrl = imageUrl,
                    Variant = variant
                });
            }
            else
            {
                // Incrementa a quantidade do item já existente
                itemInCart.Quantity++;
            }

            // Atualiza o carrinho na sessão
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Redireciona para a tela do carrinho
            return RedirectToAction("Carrinho");
        }

        // Exibe os itens no carrinho
        [HttpGet]
        public IActionResult Carrinho()
        {
            // Obtém os itens do carrinho da sessão
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart); // Exibe a view do carrinho
        }

        // Exibe a tela de pagamento
        [HttpGet]
        public async Task<IActionResult> Pagamento()
        {
            // Verifica se o usuário está logado
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null)
            {
                return RedirectToAction("Login", "Auth"); // Redireciona para login se não estiver logado
            }

            // Obtém o carrinho da sessão
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["Error"] = "Seu carrinho está vazio. Adicione itens antes de prosseguir.";
                return RedirectToAction("Carrinho");
            }

            ViewBag.TotalCarrinho = cart.Sum(item => item.Price * item.Quantity) + 50;

            // Obtém os endereços e cartões do usuário
            try
            {
                var enderecosResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Endereco/conta/{contaId}");
                if (enderecosResponse.IsSuccessStatusCode)
                {
                    var enderecos = await JsonSerializer.DeserializeAsync<List<Endereco>>(
                        await enderecosResponse.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.Enderecos = enderecos;
                }

                var cartoesResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Cartao/conta/{contaId}");
                if (cartoesResponse.IsSuccessStatusCode)
                {
                    var cartoes = await JsonSerializer.DeserializeAsync<List<Cartao>>(
                        await cartoesResponse.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    ViewBag.Cartoes = cartoes;
                }
            }
            catch
            {
                ViewBag.Enderecos = new List<Endereco>();
                ViewBag.Cartoes = new List<Cartao>();
            }

            return View(cart);
        }

        // Redireciona para cadastrar um novo endereço
        [HttpGet]
        public IActionResult NovoEndereco(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Salva um novo endereço
        [HttpPost]
        public async Task<IActionResult> NovoEndereco(Endereco endereco, string returnUrl = null)
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            endereco.ContaId = contaId.Value;
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Endereco", endereco);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl); // Redireciona para o pagamento
            }

            return RedirectToAction("Pagamento", new { erro = "Erro ao cadastrar endereço." });
        }

        // Redireciona para cadastrar um novo cartão
        [HttpGet]
        public IActionResult NovoCartao(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Salva um novo cartão
        [HttpPost]
        public async Task<IActionResult> NovoCartao(Cartao cartao, string returnUrl = null)
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            cartao.ContaId = contaId.Value;
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Cartao", cartao);

            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl); // Redireciona para o pagamento
            }

            return RedirectToAction("Pagamento", new { erro = "Erro ao cadastrar cartão." });
        }

        // Exibe a lista de pedidos
        [HttpGet]
        public async Task<IActionResult> MeusPedidos()
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var pedidosResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/Pedidos/conta/{contaId}");
                if (pedidosResponse.IsSuccessStatusCode)
                {
                    var pedidos = await JsonSerializer.DeserializeAsync<List<PedidoModel>>(
                        await pedidosResponse.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(pedidos);
                }
            }
            catch
            {
                TempData["Error"] = "Erro ao carregar os pedidos.";
            }

            return View(new List<PedidoModel>());
        }
    }
}
