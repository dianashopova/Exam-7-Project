using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeServices.RecipeVM
{
    public class RecipeIngredientViewModel
    {
        public string IngredientName { get; set; }
        public double Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
