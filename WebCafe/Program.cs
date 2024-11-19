var builder = WebApplication.CreateBuilder(args);

// Adicionar suporte para cache distribuído (necessário para sessões)
builder.Services.AddDistributedMemoryCache();

// Configurar sessões
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Aumenta a segurança dos cookies
    options.Cookie.IsEssential = true; // Marca o cookie como essencial
});

// Adicionar suporte para HttpClient
builder.Services.AddHttpClient();

// Adicionar suporte para MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configuração do pipeline de middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar sessões
app.UseSession();

app.UseAuthorization();

// Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
