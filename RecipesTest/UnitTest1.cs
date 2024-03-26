using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RecipeData;
using RecipeData.Models;
using RecipeModels;
using RecipeServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeServices.Tests
{
    [TestFixture]
    public class RecipeServiceTests
    {
        private RecipeDbContext _dbContext;
        private RecipeService _recipeService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new RecipeDbContext(options);
            SeedTestData();

            _recipeService = new RecipeService(_dbContext);
        }

        private void SeedTestData()
        {
            _dbContext.Categories.AddRange(
                new Category { Name = "Category1" },
                new Category { Name = "Category2" }
            );

            _dbContext.SaveChanges();

            _dbContext.Recipes.AddRange(
                new Recipe { Name = "Recipe1", Description = "Description1", CategoryId = 1 },
                new Recipe { Name = "Recipe2", Description = "Description2", CategoryId = 2 }
            );

            _dbContext.SaveChanges();
        }

        [Test]
        public void CreateRecipe_WithValidData_ShouldAddToDatabase()
        {
            // Arrange
            string recipeName = "New Recipe";
            string description = "Description";
            string categoryName = "Category1";
            var ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { Ingredient = new Ingredient { Name = "Ingredient1" }, Quantity = 1, Unit = "unit1" },
                new RecipeIngredient { Ingredient = new Ingredient { Name = "Ingredient2" }, Quantity = 2, Unit = "unit2" }
            };

            // Act
            _recipeService.CreateRecipe(recipeName, ingredients, description, categoryName);

            // Assert
            var recipes = _dbContext.Recipes.ToList();
            Assert.IsTrue(recipes.Any(r => r.Name == recipeName));
            Assert.AreEqual(3, recipes.Count);
            // Add more assertions as needed
        }

        [Test]
        public void DeleteRecipe_WhenExistingRecipe_ShouldRemoveFromDatabase()
        {
            // Arrange
            int recipeId = 1;

            // Act
            _recipeService.DeleteRecipe(recipeId);

            // Assert
            var recipes = _dbContext.Recipes.ToList();
            Assert.IsFalse(recipes.Any(r => r.Id == recipeId));
            Assert.AreEqual(1, recipes.Count);
            // Add more assertions as needed
        }

        [Test]
        public void GetAllRecipes_ShouldReturnAllRecipes_WhenRecipesExist()
        {
            // Act
            var recipes = _recipeService.GetAllRecipes();

            // Assert
            Assert.AreEqual(2, recipes.Count());
            // Add more assertions as needed
        }

        [Test]
        public void GetRecipeById_WhenExistingRecipeId_ShouldReturnRecipe()
        {
            // Arrange
            int existingRecipeId = 1;

            // Act
            var recipe = _recipeService.GetRecipeById(existingRecipeId);

            // Assert
            Assert.IsNotNull(recipe);
            Assert.AreEqual(existingRecipeId, recipe.Id);
            // Add more assertions as needed
        }

        [Test]
        public void GetRecipeById_WhenNonExistingRecipeId_ShouldReturnNull()
        {
            // Arrange
            int nonExistingRecipeId = 999;

            // Act
            var recipe = _recipeService.GetRecipeById(nonExistingRecipeId);

            // Assert
            Assert.IsNull(recipe);
            // Add more assertions as needed
        }

        [Test]
        public void UpdateRecipe_WhenExistingRecipe_ShouldUpdateRecipe()
        {
            // Arrange
            var existingRecipe = _dbContext.Recipes.First();
            var updatedRecipe = new Recipe
            {
                Id = existingRecipe.Id,
                Name = "Updated Recipe",
                Description = "Updated Description",
                CategoryId = existingRecipe.CategoryId
            };

            // Act
            _recipeService.UpdateRecipe(updatedRecipe);

            // Assert
            var recipe = _dbContext.Recipes.Find(existingRecipe.Id);
            Assert.IsNotNull(recipe);
            Assert.AreEqual(updatedRecipe.Name, recipe.Name);
            Assert.AreEqual(updatedRecipe.Description, recipe.Description);
            // Add more assertions as needed
        }

        // Add more tests for other methods

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
