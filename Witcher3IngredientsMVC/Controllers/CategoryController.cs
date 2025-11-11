using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Witcher3IngredientsMVC.Data;
using Witcher3IngredientsMVC.Models;

namespace Witcher3IngredientsMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories=await dbContext.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Add()
        {
            return View("Edit",new Category());  
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category=await dbContext.Categories.FindAsync(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0) 
                    await dbContext.Categories.AddAsync(category);
                else
                    dbContext.Categories.Update(category); 
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                 return View(category);
            }
        }
    }
}
