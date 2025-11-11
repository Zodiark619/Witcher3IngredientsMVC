using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Witcher3IngredientsMVC.Models.ViewModels
{
    public class ItemUpsertViewModel
    {
        public int Id { get;set; }
        public string Name { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int SelectedCategory { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> DismantleIntos { get; set; }

        public int[] SelectedDismantleIntos { get; set; } = Array.Empty<int>();
       
    }
}
