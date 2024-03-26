using RecipeModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeData.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.RecipeIngredients = new HashSet<RecipeIngredient>(); 
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]

        public double Quantity { get; set; }

        public string? Unit { get; set; }

        // Navigation property for Recipes (many-to-many)
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}