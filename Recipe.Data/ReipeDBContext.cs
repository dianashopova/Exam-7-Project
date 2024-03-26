using Microsoft.EntityFrameworkCore;
using RecipeData.Models;
using RecipeModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;

namespace RecipeData
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext() { }

        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=MyRecipeCatalogue;Integrated Security=true;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between Recipe and Ingredient
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            // Ensure data seeding for Ingredient and Category
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "spaghetti", Quantity = 1, Unit = "kg" });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Zakuska" });

            
        }
    }
}