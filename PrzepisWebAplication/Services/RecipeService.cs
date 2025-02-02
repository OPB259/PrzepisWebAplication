using Przepisy.Data;
using Przepisy.Data.Entities;
using PrzepisyWebApplication.Mappers;
using PrzepisyWebApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace PrzepisyWebApplication.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly PrzepisyContext _context;

        public RecipeService(PrzepisyContext context)
        {
            _context = context;
        }

        public int Add(RecipeViewModel item)
        {
            var entity = RecipeMapper.ToEntity(item);
            _context.Recipes.Add(entity);
            _context.SaveChanges();
            return entity.Id; // Zwrocimy nowy ID
        }

        public void Delete(int id)
        {
            var entity = _context.Recipes.Find(id);
            if (entity != null)
            {
                _context.Recipes.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(RecipeViewModel item)
        {
            var entity = RecipeMapper.ToEntity(item);
            _context.Recipes.Update(entity);
            _context.SaveChanges();
        }

        public List<RecipeViewModel> FindAll()
        {
            // pobieramy wszystkie encje i mapujemy na ViewModel
            return _context.Recipes
                .Select(r => RecipeMapper.FromEntity(r))
                .ToList();
        }

        public RecipeViewModel FindById(int id)
        {
            var entity = _context.Recipes.Find(id);
            if (entity == null)
                return null; // lub zrób throw new Exception

            return RecipeMapper.FromEntity(entity);
        }
    }
}