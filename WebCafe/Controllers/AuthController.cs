using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://192.168.15.13:5257/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Exibe a página de login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml"); // Certifica-se de buscar a view na pasta Home
        }

        // Processa o formulário de login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Dados inválidos.";
                return View("~/Views/Home/Login.cshtml"); // Certifica-se de buscar a view na pasta Home
            }

            var response = await _httpClient.PostAsJsonAsync("Conta/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                var userResponse = await JsonSerializer.DeserializeAsync<UserResponse>(
                    await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (userResponse != null && userResponse.NomeCompleto != null)
                {
                    // Salva os dados do usuário na sessão
                    HttpContext.Session.SetString("NomeCompleto", userResponse.NomeCompleto);
                    HttpContext.Session.SetInt32("ContaId", userResponse.ContaId);

                    return RedirectToAction("Index", "Home"); // Redireciona após login bem-sucedido
                }

                ViewBag.ErrorMessage = "Erro: Resposta inválida da API.";
                return View("~/Views/Home/Login.cshtml");
            }

            ViewBag.ErrorMessage = "Falha ao realizar login. Verifique suas credenciais.";
            return View("~/Views/Home/Login.cshtml");
        }

        // Exibe a página de registro (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml"); // Define onde está a view de registro
        }

        // Processa o registro de novos usuários (POST)
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Dados inválidos.";
                return View("~/Views/Home/Register.cshtml"); // Certifica-se de buscar a view de registro
            }

            var response = await _httpClient.PostAsJsonAsync("Conta", registerModel);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth"); // Redireciona para a tela de login
            }

            ViewBag.ErrorMessage = "Erro ao realizar cadastro. Tente novamente.";
            return View("~/Views/Home/Register.cshtml");
        }

        // Realiza logout (GET)
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpa a sessão do usuário
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
        }
    }
}
