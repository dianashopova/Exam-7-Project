using RecipeData;
using RecipeData.Models;
using RecipeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RecipeServices
{
    /// <summary>
    /// Service class for managing recipes.
    /// </summary>
    public class RecipeService : IRecipeService
    {
        private readonly RecipeDbContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RecipeService(RecipeDbContext dbContext)
        {
            db = dbContext;
        }

        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="name">The name of the recipe.</param>
        /// <param name="ingredients">The list of ingredients for the recipe.</param>
        /// <param name="description">The description of the recipe.</param>
        /// <param name="categoryName">The name of the category to which the recipe belongs.</param>
        public void CreateRecipe(string name, List<RecipeIngredient> ingredients, string description, string categoryName)
        {
            // Check if the category already exists
            Category category = db.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
            {
                // If category doesn't exist, create a new one
                category = new Category { Name = categoryName };
            }

            // Create a new recipe
            var recipe = new Recipe
            {
                Name = name,
                Description = description,
                Category = category // Assign the category to the recipe
            };

            // Add ingredients to the recipe
            foreach (var recipeIngredient in ingredients)
            {
                // Check if the ingredient already exists
                Ingredient existingIngredient = db.Ingredients.FirstOrDefault(i => i.Name == recipeIngredient.Ingredient.Name);
                if (existingIngredient == null)
                {
                    // If ingredient doesn't exist, add it to the context
                    existingIngredient = new Ingredient { Name = recipeIngredient.Ingredient.Name };
                }

                // Create RecipeIngredient with the existing or new ingredient
                RecipeIngredient newRecipeIngredient = new RecipeIngredient
                {
                    Ingredient = existingIngredient,
                    Quantity = recipeIngredient.Quantity,
                    Unit = recipeIngredient.Unit
                };

                // Add the RecipeIngredient to the recipe
                recipe.RecipeIngredients.Add(newRecipeIngredient);
            }

            // Add the recipe to the context and save changes
            db.Recipes.Add(recipe);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to delete.</param>
        public void DeleteRecipe(int id)
        {
            var recipe = db.Recipes.Find(id);
            if (recipe != null)
            {
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves all recipes.
        /// </summary>
        /// <returns>An enumerable collection of recipes.</returns>
        public IEnumerable<Recipe> GetAllRecipes()
        {
            return db.Recipes.ToList();
        }

        /// <summary>
        /// Retrieves a recipe by its ID.
        /// </summary>
        /// <param name="id">The ID of the recipe to retrieve.</param>
        /// <returns>The recipe with the specified ID, or null if not found.</returns>
        public Recipe GetRecipeById(int id)
        {
            return db.Recipes.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Updates an existing recipe.
        /// </summary>
        /// <param name="recipe">The recipe to update.</param>
        /// <remarks>
        /// This method updates an existing recipe in the database with the provided recipe object.
        /// It first finds the existing recipe by its ID and then detaches it from the context to avoid
        /// conflicts with entity tracking. After detaching, the method updates the recipe object
        /// with the EntityState.Modified flag, indicating that it should be marked as modified in the context.
        /// Finally, it saves the changes to the database.
        /// </remarks>
        public void UpdateRecipe(Recipe recipe)
        {
            var existingRecipe = db.Recipes.Find(recipe.Id);
            if (existingRecipe != null)
            {
                // Detach existing recipe
                db.Entry(existingRecipe).State = EntityState.Detached;

                // Update the recipe
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
