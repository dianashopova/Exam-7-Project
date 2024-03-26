using RecipeData;
using RecipeData.Models;
using RecipeModels;
using RecipeServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeConsole
{
    public class Display
    {
        private int closeOperationId = 6;
        private readonly RecipeService recipeService;
        private readonly RecipeDbContext db;

        public Display()
        {
            this.db = new RecipeDbContext();
            db.Database.EnsureCreated();
            this.recipeService = new RecipeService(db);
            Input();
        }

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 15) + "MENU" + new string(' ', 15));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. List all recipes");
            Console.WriteLine("2. Add new recipe");
            Console.WriteLine("3. Update recipe");
            Console.WriteLine("4. List recipe details by ID");
            Console.WriteLine("5. Delete recipe");
            Console.WriteLine("6. Exit");
        }

        private void Input()
        {
            var operation = -1;
            do
            {
                ShowMenu();
                operation = int.Parse(Console.ReadLine());
                switch (operation)
                {
                    case 1:
                        ListAllRecipes();
                        break;
                    case 2:
                        AddRecipe();
                        break;
                    case 3:
                        UpdateRecipe();
                        break;
                    case 4:
                        FetchRecipe();
                        break;
                    case 5:
                        DeleteRecipe();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }

       /* private void SearchRecipesByIngredient()
        {
            Console.WriteLine("Ingredient to search for:");
            string ingredient = Console.ReadLine();
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 3)
                + "RECIPES CONTAINING YOUR INGREDIENT:" + new string(' ', 3));
            Console.WriteLine(new string('-', 40));

            var recipes = recipeService.SearchByIngredient(ingredient);

            if (recipes.Any())
            {
                foreach (var recipe in recipes)
                {
                    Console.WriteLine($"- {recipe.Name}");
                }
            }
            else
            {
                Console.WriteLine($"No recipes found containing {ingredient}.");
            }
        }*/

        private void AddRecipe()
        {
            Console.WriteLine("Enter recipe name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter category name: ");
            string categoryName = Console.ReadLine();
            List<RecipeIngredient> ingredients = GetIngredients();
            Console.WriteLine("Enter recipe instructions: ");
            string description = Console.ReadLine();
            

            recipeService.CreateRecipe(name, ingredients, description, categoryName);
            Console.WriteLine("Recipe added successfully");
        }

        private void DeleteRecipe()
        {
            Console.WriteLine("Enter recipe id to delete: ");
            int recipeID = int.Parse(Console.ReadLine());
            //TO DO ako nqma takava recepta sa kazva 
            // otherwise Recipe deleted
            recipeService.DeleteRecipe(recipeID);
        }

        private void ListAllRecipes()
        {

            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 15) + "RECIPES" + new string(' ', 15));
            Console.WriteLine(new string('-', 40));
            var recipes = recipeService.GetAllRecipes();

            if (recipes.Any())
            {
                foreach (var recipe in recipes)
                {
                    // Check if recipe.Category is not null before accessing its Name property
                    string categoryName = recipe.Category != null ? recipe.Category.Name : "Uncategorized";

                    Console.WriteLine($"{recipe.Id}) {recipe.Name} ({categoryName})");
                }
            }
            else
            {
                Console.WriteLine("There are no recipes");
            }
        }

        private void UpdateRecipe()
        {
            Console.WriteLine("Enter recipe ID to update: ");
            int id = int.Parse(Console.ReadLine());
            var recipe = recipeService.GetRecipeById(id);
            if (recipe != null)
            {
                Console.WriteLine("Enter new recipe name: ");
                string newName = Console.ReadLine();
                Console.WriteLine("Enter new recipe description: ");
                string newDescription = Console.ReadLine();

                recipe.Name = newName;
                recipe.Description = newDescription;

                recipeService.UpdateRecipe(recipe);
                Console.WriteLine("Recipe updated successfully");
            }
            else
            {
                Console.WriteLine("Recipe not found");
            }
        }

        private void FetchRecipe()
        {
            Console.WriteLine("Enter recipe ID to fetch: ");
            int id = int.Parse(Console.ReadLine());
            var recipe = recipeService.GetRecipeById(id);
            if (recipe != null)
            {
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"ID: {recipe.Id}");
                Console.WriteLine($"Name: {recipe.Name}");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in recipe.RecipeIngredients)
                {
                    Console.WriteLine($"- {ingredient.Ingredient.Name}: {ingredient.Quantity} {ingredient.Unit}");
                }
                Console.WriteLine($"Instructions: {recipe.Description}");
                Console.WriteLine($"Category: {recipe.Category.Name}");
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                Console.WriteLine("Recipe not found");
            }
        }

        private List<RecipeIngredient> GetIngredients()
        {
            List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

            // You can prompt the user to enter the number of ingredients and details for each ingredient
            // This method should only collect user input and return the data without performing any business logic

            Console.WriteLine("Enter ingredients (type 'DONE' to finish):");

            while (true)
            {
                Console.Write("Enter ingredient details (name quantity unit): ");
                string input = Console.ReadLine();

                // Check if the user typed 'DONE' to finish adding ingredients
                if (input.ToUpper() == "DONE")
                {
                    break; // Exit the loop
                }

                // Split the input by space to get individual parts
                string[] parts = input.Split(' ');

                // Check if input contains all three parts
                if (parts.Length != 3)
                {
                    Console.WriteLine("Invalid input format. Please enter the name, quantity, and unit separated by space.");
                    continue; // Skip to next iteration of the loop
                }

                // Extract individual parts
                string ingredientName = parts[0];
                double quantity;
                if (!double.TryParse(parts[1], out quantity))
                {
                    Console.WriteLine("Invalid quantity. Please enter a valid number.");
                    continue; // Skip to next iteration of the loop
                }
                string unit = parts[2];

                // Create a new RecipeIngredient object with user inputs
                RecipeIngredient ingredient = new RecipeIngredient
                {
                    Ingredient = new Ingredient { Name = ingredientName },
                    Quantity = quantity,
                    Unit = unit
                };

                // Add the ingredient to the list
                ingredients.Add(ingredient);
            }

            return ingredients;
        }
    }
}
