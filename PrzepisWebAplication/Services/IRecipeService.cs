using PrzepisyWebApplication.Models;
using System.Collections.Generic;

namespace PrzepisyWebApplication.Services
{
    public interface IRecipeService
    {
        int Add(RecipeViewModel item);
        void Delete(int id);
        void Update(RecipeViewModel item);
        List<RecipeViewModel> FindAll();
        RecipeViewModel FindById(int id);
    }
}