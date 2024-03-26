using RecipeModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeData.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>(); 
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(500)] 
        public string Description { get; set; }

        // Navigation property for Ingredients (many-to-many)
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        // Navigation property for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}