using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Przepisy.Data.Entities
{
    [Table("comment")]
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }  // treść komentarza

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // (Opcjonalnie) klucz obcy do przepisu, by komentarz był powiązany z Recipe:
        public int? RecipeId { get; set; }
        public RecipeEntity Recipe { get; set; }
    }
}