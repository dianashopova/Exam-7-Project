using RecipeData.Models;
using RecipeModels;
using System.ComponentModel.DataAnnotations;

namespace RecipeCatalogueWeb.Models
{
    public class BindingRecipe
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation property for Ingredients (many-to-many)
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        // Navigation property for Category
        public int CategoryId { get; set; }
    }
}
