using System.ComponentModel.DataAnnotations; // przyda się do walidacji

namespace PrzepisyWebApplication.Models
{
    public class RecipeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Recipe title is missing!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is missing!")]
        public string Description { get; set; }

        // ewentualnie dodatkowe pola:
       
    }
}