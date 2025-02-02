using Microsoft.AspNetCore.Mvc;
using PrzepisyWebApplication.Models;
using PrzepisyWebApplication.Services;

namespace PrzepisyWebApplication.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var all = _recipeService.FindAll();

            if (!string.IsNullOrEmpty(search))
            {
                // prosty filtr po tytule
                all = all.Where(r => r.Title.Contains(search, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(all);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RecipeViewModel recipe)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }
            _recipeService.Add(recipe);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _recipeService.FindById(id);
            if (item == null) return NotFound();

            return View("Edit", item);
            // lub sam "Edit" w nazwie, 
            // zależnie jak nazwałeś plik widoku
        }

        [HttpPost]
        public IActionResult Edit(RecipeViewModel recipe)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", recipe);
            }
            _recipeService.Update(recipe);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var item = _recipeService.FindById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = _recipeService.FindById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _recipeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}