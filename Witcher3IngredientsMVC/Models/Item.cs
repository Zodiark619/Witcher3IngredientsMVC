using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Witcher3IngredientsMVC.Models
{
    public class Category
    {
        

        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Item
    {
      
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
       public Category Category { get; set; }
        
        public List<ItemLink> DismantleIntoLinks { get; set; } = new();
        

    }
    public class ItemLink
    {
        public int Id { get; set; }
        public int ResultItemId { get; set; }  // e.g., Junk1
        public Item ResultItem { get; set; }

        
    }
}
