using Microsoft.EntityFrameworkCore;
using Przepisy.Data.Entities;

namespace Przepisy.Data
{
    public class PrzepisyContext : DbContext
    {
        public PrzepisyContext(DbContextOptions<PrzepisyContext> options)
            : base(options)
        {
        }

       

        // DbSet reprezentuje tabelę w bazie danych
        public DbSet<RecipeEntity> Recipes { get; set; }

        // W przyszłości możesz dodać więcej DbSet, np. DbSet<IngredientEntity> ...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tutaj możesz skonfigurować wstępne dane
            // np. modelBuilder.Entity<RecipeEntity>().HasData(
            //    new RecipeEntity { Id = 1, Title = "Sample", Description = "Sample desc" }
            // );
        }
    }
}