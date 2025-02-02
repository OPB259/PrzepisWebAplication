using Przepisy.Data.Entities;
using PrzepisyWebApplication.Models;

namespace PrzepisyWebApplication.Mappers
{
    public static class RecipeMapper
    {
        public static RecipeViewModel FromEntity(RecipeEntity entity)
        {
            return new RecipeViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
        }

        public static RecipeEntity ToEntity(RecipeViewModel model)
        {
            return new RecipeEntity
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description
            };
        }
    }
}