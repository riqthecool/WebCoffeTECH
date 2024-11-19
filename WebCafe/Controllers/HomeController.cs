using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://192.168.15.13:5257/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index() => View();

        public IActionResult SobreNos() => View();

        public IActionResult Portfolio() => View();

        public IActionResult NossaMissao() => View();

        public IActionResult DuvidasFrequentes() => View();

        public IActionResult FaleConosco() => View();

        public async Task<IActionResult> MeuPerfil()
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var response = await _httpClient.GetAsync($"Conta/{contaId}");
                if (response.IsSuccessStatusCode)
                {
                    var conta = await JsonSerializer.DeserializeAsync<Conta>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(conta);
                }

                ViewBag.ErrorMessage = "Erro ao carregar os dados.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> MeusCartoes()
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var response = await _httpClient.GetAsync($"Cartao/conta/{contaId}");
                if (response.IsSuccessStatusCode)
                {
                    var cartoes = await JsonSerializer.DeserializeAsync<List<Cartao>>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(cartoes);
                }

                ViewBag.ErrorMessage = "Erro ao carregar os cartões.";
                return View(new List<Cartao>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View(new List<Cartao>());
            }
        }

        public IActionResult NovoCartao()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarCartao(Cartao cartao)
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                cartao.ContaId = contaId.Value;
                cartao.Conta = null;

                var content = new StringContent(
                    JsonSerializer.Serialize(cartao),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("Cartao", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cartão adicionado com sucesso!";
                    return RedirectToAction("MeusCartoes");
                }

                ViewBag.ErrorMessage = $"Erro ao adicionar o cartão. Status Code: {response.StatusCode}";
                return View("NovoCartao", cartao);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View("NovoCartao", cartao);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirCartao(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Cartao/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cartão excluído com sucesso!";
                    return RedirectToAction("MeusCartoes");
                }

                TempData["ErrorMessage"] = $"Erro ao excluir o cartão. Status Code: {response.StatusCode}";
                return RedirectToAction("MeusCartoes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro: {ex.Message}";
                return RedirectToAction("MeusCartoes");
            }
        }

        public async Task<IActionResult> MeusDados()
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var response = await _httpClient.GetAsync($"Conta/{contaId}");
                if (response.IsSuccessStatusCode)
                {
                    var conta = await JsonSerializer.DeserializeAsync<Conta>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(conta);
                }

                ViewBag.ErrorMessage = "Erro ao carregar os dados do usuário.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarDados(Conta conta)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(conta),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"Conta/{conta.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Dados atualizados com sucesso!";
                    return RedirectToAction("MeusDados");
                }

                ViewBag.ErrorMessage = $"Erro ao atualizar os dados. Status Code: {response.StatusCode}";
                return View("MeusDados", conta);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View("MeusDados", conta);
            }
        }

        public async Task<IActionResult> MeusEnderecos()
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                var response = await _httpClient.GetAsync($"Endereco/conta/{contaId}");
                if (response.IsSuccessStatusCode)
                {
                    var enderecos = await JsonSerializer.DeserializeAsync<List<Endereco>>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return View(enderecos);
                }

                ViewBag.ErrorMessage = "Erro ao carregar os endereços.";
                return View(new List<Endereco>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View(new List<Endereco>());
            }
        }

        public IActionResult NovoEndereco()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarEndereco(Endereco endereco)
        {
            int? contaId = HttpContext.Session.GetInt32("ContaId");
            if (contaId == null) return RedirectToAction("Login", "Auth");

            try
            {
                endereco.ContaId = contaId.Value;

                var content = new StringContent(
                    JsonSerializer.Serialize(endereco),
                    System.Text.Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("Endereco", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Endereço adicionado com sucesso!";
                    return RedirectToAction("MeusEnderecos");
                }

                ViewBag.ErrorMessage = $"Erro ao adicionar o endereço. Status Code: {response.StatusCode}";
                return View("NovoEndereco", endereco);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro: {ex.Message}";
                return View("NovoEndereco", endereco);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirEndereco(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Endereco/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Endereço excluído com sucesso!";
                    return RedirectToAction("MeusEnderecos");
                }

                TempData["ErrorMessage"] = $"Erro ao excluir o endereço. Status Code: {response.StatusCode}";
                return RedirectToAction("MeusEnderecos");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro: {ex.Message}";
                return RedirectToAction("MeusEnderecos");
            }
        }
    }
}
