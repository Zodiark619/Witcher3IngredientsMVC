using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Witcher3IngredientsMVC.Data;
using Witcher3IngredientsMVC.Models;
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
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(RecipeUpsertViewModel model)
        {
            if (!model.Requirements.Any(r => r.ItemId != 0 && r.Quantity > 0))
            {
                var items = await dbContext.Items
    .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name })
    .ToListAsync();

                foreach (var req in model.Requirements)
                    req.AvailableItems = items;
                ModelState.AddModelError("", "Please add at least one requirement.");
                return View("Edit", model);
            }
            if (!ModelState.IsValid)
            {
                var items = await dbContext.Items
    .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name })
    .ToListAsync();

                foreach (var req in model.Requirements)
                    req.AvailableItems = items;
                return View(model);

            }
            if (model.Id != 0)
            {

            
            var recipe = await dbContext.Recipes
                .Include(r => r.Requirements)
                .FirstOrDefaultAsync(r => r.Id == model.Id);

            if (recipe == null)
                return NotFound();

            // Update basic fields
            recipe.Name = model.Name;

            // Remove old requirements
            dbContext.RecipeRequirements.RemoveRange(recipe.Requirements);

            // Add new ones from the ViewModel
            recipe.Requirements = model.Requirements
                .Where(r => r.ItemId != 0 && r.Quantity > 0) // skip empty rows
                .Select(r => new RecipeRequirement
                {
                    RecipeId = recipe.Id,
                    ItemId = r.ItemId,
                    Quantity = r.Quantity
                })
                .ToList();
            }
            else
            {
                var created = new Recipe
                {
                    Name = model.Name,
                    Requirements= model.Requirements
                .Where(r => r.ItemId != 0 && r.Quantity > 0) // skip empty rows
                .Select(r => new RecipeRequirement
                {
                   
                    ItemId = r.ItemId,
                    Quantity = r.Quantity
                })
                .ToList()
                };
                await dbContext.Recipes.AddAsync(created);
            }
                await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await dbContext.Recipes

         .Include(x => x.Requirements)
             .ThenInclude(link => link.Item)

        .FirstOrDefaultAsync(i => i.Id == id);

            if (recipe != null)
            {
                // Remove related ItemLinks first

                dbContext.RecipeRequirements.RemoveRange(recipe.Requirements);

                dbContext.Recipes.Remove(recipe);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
