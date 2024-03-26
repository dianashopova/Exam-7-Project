using RecipeData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeModels
{
    public class RecipeIngredient
    {
        //many-to-many relationship between Recipe and Ingredient
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; }

        public string? Unit { get; set; }
        public double Quantity { get; set; }


    }
}
