using Microsoft.AspNetCore.Mvc;
using PrzepisyWebApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace PrzepisyWebApplication.Controllers
{
    public class RecipeController : Controller
    {
        // Baza w pamięci podreczej
        static List<RecipeViewModel> recipes = new List<RecipeViewModel>();

        [HttpGet]
        public IActionResult Index(string search)
        {
            // Kopia głównej listy
            var filteredRecipes = recipes;

            if (!string.IsNullOrEmpty(search))
            {
                filteredRecipes = recipes
                    .Where(r => r.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(filteredRecipes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Pusty formularz
            return View();
        }

        [HttpPost]
        public IActionResult Create(RecipeViewModel recipe)
        {
            if (ModelState.IsValid)
            {
                // Prosty sposób na ustalenie nowego Id
                // o ile lista nie jest pusta
                if (recipes.Count > 0)
                    recipe.Id = recipes.Max(x => x.Id) + 1;
                else
                    recipe.Id = 1;

                recipes.Add(recipe);

                // Po dodaniu wracamy do listy
                return RedirectToAction("Index");
            }

            // Jeśli walidacja nie przeszła, zwróć ten sam widok z błędami
            return View(recipe);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var recipe = recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound(); // lub RedirectToAction("Index")
            }

            return View(recipe);
        }

        [HttpPost]
        public IActionResult Edit(RecipeViewModel updatedRecipe)
        {
            if (!ModelState.IsValid)
            {
                // Błędy walidacji – wróć do widoku z danymi
                return View(updatedRecipe);
            }

            // Znajdź istniejący przepis
            var recipe = recipes.FirstOrDefault(r => r.Id == updatedRecipe.Id);
            if (recipe == null)
            {
                return NotFound(); // lub RedirectToAction("Index")
            }

            // Nadpisz wartości
            recipe.Title = updatedRecipe.Title;
            recipe.Description = updatedRecipe.Description;
            // recipe.Whatever = updatedRecipe.Whatever; // jeśli masz więcej pól

            // Przekieruj do listy
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var recipe = recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var recipe = recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var recipe = recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null)
            {
                recipes.Remove(recipe);
            }
            return RedirectToAction("Index");
        }
    }
}