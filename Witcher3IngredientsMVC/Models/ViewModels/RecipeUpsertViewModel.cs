using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Witcher3IngredientsMVC.Models.ViewModels
{
    public class RecipeUpsertViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<RecipeRequirementViewModel> Requirements { get; set; } = new();
    }
    public class RecipeRequirementViewModel
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AvailableItems { get; set; } = new List<SelectListItem>();
    }
}
