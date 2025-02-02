using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Przepisy.Data;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja kontrolerów + widoków (MVC)
builder.Services.AddControllersWithViews();

// Rejestracja DbContext (PrzepisyContext) z u¿yciem SQLite
builder.Services.AddDbContext<PrzepisyContext>(options =>
{
    // Œcie¿ka do pliku Recipes.db (w folderze uruchomieniowym)
    var dbPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Recipes.db");
    options.UseSqlite($"Data Source={dbPath}");
});



var app = builder.Build();

// Obs³uga b³êdów w trybie produkcyjnym
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

}


app.UseStaticFiles();
app.UseRouting();


// Domyœlna trasa MVC (Home/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Uruchom aplikacjê
app.Run();