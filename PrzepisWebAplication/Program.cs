using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Przepisy.Data;
using PrzepisyWebApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja kontroler�w + widok�w (MVC)
builder.Services.AddControllersWithViews();

// Rejestracja DbContext (PrzepisyContext) z u�yciem SQLite
builder.Services.AddDbContext<PrzepisyContext>(options =>
{
    // �cie�ka do pliku Recipes.db (w folderze uruchomieniowym)
    var dbPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Recipes.db");
    options.UseSqlite($"Data Source={dbPath}");
});

builder.Services.AddTransient<IRecipeService, RecipeService>();


var app = builder.Build();

// Obs�uga b��d�w w trybie produkcyjnym
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

}


app.UseStaticFiles();
app.UseRouting();


// Domy�lna trasa MVC (Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Uruchom aplikacj�
app.Run();