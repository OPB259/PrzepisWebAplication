using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Przepisy.Data;
using PrzepisyWebApplication.Middlewares; // <-- Namespace z Twoimi plikami middleware
using PrzepisyWebApplication.Services;    // <-- Je�li rejestrujesz IRecipeService
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 1. Rejestracja kontroler�w (MVC)
builder.Services.AddControllersWithViews();

// 2. Rejestracja DbContext (PrzepisyContext) - SQLite
var dbPath = Path.Combine(Environment.CurrentDirectory, "Recipes.db");
builder.Services.AddDbContext<PrzepisyContext>(options =>
{
    options.UseSqlite($"Data Source={dbPath}");
});

// 3. Rejestracja serwis�w (np. IRecipeService -> RecipeService)
builder.Services.AddTransient<IRecipeService, RecipeService>();

var app = builder.Build();

// 4. Obs�uga b��d�w w trybie produkcyjnym
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts(); // je�li chcesz w��czy� HSTS w produkcji
}

// 5. Middleware do logowania ��da�/odpowiedzi
app.Use(async (context, next) =>
{
    Console.WriteLine("=== Request Info ===");
    Console.WriteLine($"Method: {context.Request.Method}");
    Console.WriteLine($"Path: {context.Request.Path}");

    await next(); // przej�cie do kolejnego elementu w potoku

    Console.WriteLine("=== Response Info ===");
    Console.WriteLine($"Status: {context.Response.StatusCode}");
});

// 6. Middleware do zapisywania ostatniej wizyty (LastVisitMiddleware)
app.UseMiddleware<LastVisitMiddleware>();

// 7. Obs�uga plik�w statycznych (wwwroot)
app.UseStaticFiles();

// 8. Routing
app.UseRouting();

// 9. Mapowanie kontroler�w MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 10. Uruchom aplikacj�
app.Run();