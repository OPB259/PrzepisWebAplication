using NUnit.Framework;
using Moq;
using PrzepisyWebApplication.Controllers;
using PrzepisyWebApplication.Services;
using PrzepisyWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace PrzepisyWebApplicationTests
{
    [TestFixture]
    public class RecipeControllerTests
    {
        // Mock serwisu i instancja kontrolera
        private Mock<IRecipeService> _serviceMock;
        private RecipeController _controller;

        // [TearDown] – wywołujemy Dispose() po każdym teście (opcjonalne)
        [TearDown]
        public void Cleanup()
        {
            _controller?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            // Przykładowe dane do testów
            var sampleRecipes = new List<RecipeViewModel>
            {
                new RecipeViewModel { Id = 1, Title="Rosół",       Description="Lorem Ipsum" },
                new RecipeViewModel { Id = 2, Title="Pomidorowa", Description="Ipsum Lorem" }
            };

            // Tworzymy mock serwisu
            _serviceMock = new Mock<IRecipeService>();

            // Ustawiamy, że _serviceMock.FindAll() zawsze zwróci sampleRecipes
            _serviceMock.Setup(s => s.FindAll()).Returns(sampleRecipes);

            // Inicjalizujemy kontroler z mockiem
            _controller = new RecipeController(_serviceMock.Object);
        }

        // 1. Test Index (bez wyszukiwania)
        [Test]
        public void Index_Action_ReturnsViewWithRecipes()
        {
            // Act
            var result = _controller.Index(search: null) as ViewResult;

            // Assert
            Assert.NotNull(result, "Result is null – expected a ViewResult");
            Assert.IsInstanceOf<ViewResult>(result);

            var model = result.Model as IEnumerable<RecipeViewModel>;
            Assert.NotNull(model, "Model is null – expected a list of RecipeViewModel");
            Assert.AreEqual(2, model.Count(), "Should return 2 recipes in model");
        }

        // 2. Test Index (z wyszukiwaniem)
        //   Zakładamy, że w RecipeController.Index(string search) 
        //   filtrujesz po Title.Contains(search)
        [Test]
        public void Index_Action_WithSearch_ReturnsFilteredRecipes()
        {
            // Act
            var result = _controller.Index("Ros") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as IEnumerable<RecipeViewModel>;
            Assert.AreEqual(1, model.Count(), "Powinien zostać tylko 'Rosół'");
            Assert.AreEqual("Rosół", model.First().Title);
        }

        // 3. Test GET: Create
        [Test]
        public void CreateGet_ReturnsEmptyView()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result, "Result is null – expected a ViewResult");
            Assert.IsNull(result.Model, "Create GET typically has no model");
        }

        // 4. Test POST: Create – valid model
        [Test]
        public void CreatePost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newRecipe = new RecipeViewModel { Title = "Schabowy", Description = "Kotlet" };
            _serviceMock.Setup(s => s.Add(newRecipe)).Returns(5); // Załóżmy, że ID = 5

            // Act
            var result = _controller.Create(newRecipe) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result, "Result is null – expected a RedirectToActionResult");
            Assert.AreEqual("Index", result.ActionName, "Should redirect to Index after success");
            _serviceMock.Verify(s => s.Add(newRecipe), Times.Once, "Add method not called exactly once");
        }

        // 5. Test POST: Create – invalid model (np. Title = null)
        [Test]
        public void CreatePost_InvalidModel_ReturnsSameView()
        {
            // Arrange
            var invalidRecipe = new RecipeViewModel { Title = "", Description = "" };
            _controller.ModelState.AddModelError("Title", "Required"); // symulujemy błąd walidacji

            // Act
            var result = _controller.Create(invalidRecipe) as ViewResult;

            // Assert
            Assert.NotNull(result, "Result is null – expected a ViewResult");
            Assert.AreEqual(invalidRecipe, result.Model, "Should return the same model on error");
            _serviceMock.Verify(s => s.Add(It.IsAny<RecipeViewModel>()), Times.Never,
                "Add should not be called if model is invalid");
        }

        // 6. Test GET: Edit – valid ID
        [Test]
        public void EditGet_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var recipe = new RecipeViewModel { Id = 99, Title = "Bigos", Description = "Kapusta" };
            _serviceMock.Setup(s => s.FindById(99)).Returns(recipe);

            // Act
            var result = _controller.Edit(99) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreSame(recipe, result.Model, "Model should be the recipe returned from service");
        }

        // 7. Test GET: Edit – invalid (not found)
        [Test]
        public void EditGet_InvalidId_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.FindById(777)).Returns((RecipeViewModel)null);

            // Act
            var result = _controller.Edit(777);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result, "If recipe is not found, should return NotFound()");
        }

        // 8. Test POST: Edit – valid
        [Test]
        public void EditPost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var updated = new RecipeViewModel { Id = 99, Title = "Bigos", Description = "Kapusta" };

            // Act
            var result = _controller.Edit(updated) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _serviceMock.Verify(s => s.Update(updated), Times.Once);
        }

        // 9. Test POST: Edit – invalid model
        [Test]
        public void EditPost_InvalidModel_ReturnsSameView()
        {
            // Arrange
            var invalidModel = new RecipeViewModel { Id = 2, Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Edit(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(invalidModel, result.Model, "Should return same model on error");
            _serviceMock.Verify(s => s.Update(It.IsAny<RecipeViewModel>()), Times.Never);
        }

        // 10. Test GET: Delete – valid
        [Test]
        public void DeleteGet_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var recipe = new RecipeViewModel { Id = 55, Title = "Gulasz" };
            _serviceMock.Setup(s => s.FindById(55)).Returns(recipe);

            // Act
            var result = _controller.Delete(55) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreSame(recipe, result.Model);
        }

        // 11. Test GET: Delete – not found
        [Test]
        public void DeleteGet_NotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.FindById(999)).Returns((RecipeViewModel)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        // 12. Test POST: DeleteConfirmed
        [Test]
        public void DeleteConfirmed_Always_RedirectToIndex()
        {
            // Act
            var result = _controller.DeleteConfirmed(123) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _serviceMock.Verify(s => s.Delete(123), Times.Once);
        }

        // 13. Test GET: Details – found
        [Test]
        public void DetailsGet_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var recipe = new RecipeViewModel { Id = 10, Title = "Fasolka" };
            _serviceMock.Setup(s => s.FindById(10)).Returns(recipe);

            // Act
            var result = _controller.Details(10) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(recipe, result.Model);
        }

        // 14. Test GET: Details – not found
        [Test]
        public void DetailsGet_NotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.FindById(1)).Returns((RecipeViewModel)null);

            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
