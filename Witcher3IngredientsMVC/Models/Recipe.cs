namespace Witcher3IngredientsMVC.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<RecipeRequirement> Requirements { get; set; } = new();
    }
    public class RecipeRequirement
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}
