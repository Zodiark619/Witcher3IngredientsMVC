using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Witcher3IngredientsMVC.Data;
using Witcher3IngredientsMVC.Models.ViewModels;

namespace Witcher3IngredientsMVC.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public RecipeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var items = await dbContext.Recipes
                 
        .Include(x => x.Requirements)
            .ThenInclude(link => link.Item)
            
        .ToListAsync();
            return View(items);
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Add()
        {
            var items = await dbContext.Items
          .Select(i => new SelectListItem
          {
              Value = i.Id.ToString(),
              Text = i.Name
          })
          .ToListAsync();

            var viewModel = new RecipeUpsertViewModel
            {
                Requirements = new List<RecipeRequirementViewModel>
        {
            new RecipeRequirementViewModel { AvailableItems = items }
        }
            };

           

            return View("Edit", viewModel);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(int id)
        {
            var items = await dbContext.Items
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                })
                .ToListAsync();

            var recipe = await dbContext.Recipes
                .Include(x => x.Requirements)
                .ThenInclude(r => r.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (recipe == null)
                return NotFound();

            var viewModel = new RecipeUpsertViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Requirements = recipe.Requirements.Select(req => new RecipeRequirementViewModel
                {
                    ItemId = req.ItemId,
                    Quantity = req.Quantity,
                    AvailableItems = items
                }).ToList()
            };

            // If recipe has no requirements, show at least one blank row
            if (!viewModel.Requirements.Any())
            {
                viewModel.Requirements.Add(new RecipeRequirementViewModel
                {
                    AvailableItems = items
                });
            }

            return View( viewModel);
        }

    }
}
