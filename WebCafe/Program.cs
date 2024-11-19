var builder = WebApplication.CreateBuilder(args);

// Adicionar suporte para cache distribu�do (necess�rio para sess�es)
builder.Services.AddDistributedMemoryCache();

// Configurar sess�es
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Aumenta a seguran�a dos cookies
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

// Configura��o do pipeline de middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar sess�es
app.UseSession();

app.UseAuthorization();

// Configura��o das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
