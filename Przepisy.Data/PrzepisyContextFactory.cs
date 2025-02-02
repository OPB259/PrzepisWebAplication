using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace Przepisy.Data
{
    public class PrzepisyContextFactory : IDesignTimeDbContextFactory<PrzepisyContext>
    {
        public PrzepisyContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PrzepisyContext>();
            // Dokładnie ta sama konfiguracja co w Program.cs
            var path = Path.Combine(Environment.CurrentDirectory, "Recipes.db");
            builder.UseSqlite($"Data Source={path}");

            return new PrzepisyContext(builder.Options);
        }
    }
}