using RecipeData.Models;
using RecipeModels;
using RecipeServices.RecipeVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeServices
{
    public interface IRecipeService
    {
        IEnumerable<Recipe> GetAllRecipes();
        Recipe GetRecipeById(int id);
        void CreateRecipe(string name, List<RecipeIngredient> ingredients, string description, string category);
        void UpdateRecipe(Recipe recipe);
        void DeleteRecipe(int id);
    }
}
