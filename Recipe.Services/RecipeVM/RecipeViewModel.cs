using RecipeData.Models;
using RecipeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeServices.RecipeVM
{
    public class RecipeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public Category Category { get; set; }
    }
}
