using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Witcher3IngredientsMVC.Data;
using Witcher3IngredientsMVC.Models;
using Witcher3IngredientsMVC.Models.ViewModels;

namespace Witcher3IngredientsMVC.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ItemController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var items = await dbContext.Items
                .Include(x => x.Category)
       .Include(x => x.DismantleIntoLinks)
           .ThenInclude(link => link.ResultItem)
       .ToListAsync();
            return View(items);
        }

            public async Task<IActionResult> AddAsync()
            {
            var viewModel = new ItemUpsertViewModel
            {
                Categories =await dbContext.Categories.Select(x=>new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync(),
                DismantleIntos =await dbContext.Items
        .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name })
        .ToListAsync(),
            };

            return View("Edit", viewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = dbContext.Items.Include(x => x.DismantleIntoLinks).FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();
            var model = new ItemUpsertViewModel
            {
                Id = item.Id,
                Name = item.Name,
                SelectedCategory = item.CategoryId,

                Categories =await dbContext.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync(),
                DismantleIntos = await dbContext.Items
            .Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            })
            .ToListAsync(),
                SelectedDismantleIntos = item.DismantleIntoLinks.Select(d => d.ResultItemId).ToArray()
            };


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ItemUpsertViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                viewmodel.Categories = await dbContext.Categories
        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
        .ToListAsync();

                viewmodel.DismantleIntos = await dbContext.Items
                    .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name })
                    .ToListAsync();
                return View(viewmodel);

            }

            if (viewmodel.Id == 0)
            {
                // ADD
                var model = new Item
                {
                    Name = viewmodel.Name,
                    CategoryId = viewmodel.SelectedCategory
                };
                if (viewmodel.SelectedDismantleIntos?.Any() == true)
                {
                    // Dismantle Into (this item produces these)
                    foreach (var i in viewmodel.SelectedDismantleIntos)
                    {

                        model.DismantleIntoLinks.Add(new ItemLink
                        {
                            //ResultItem = model.Id,
                            ResultItemId = i
                        });
                    }

                }

                await dbContext.Items.AddAsync(model);
            }
            else
            {
                // UPDATE
                var existing = await dbContext.Items
                    .Include(x => x.DismantleIntoLinks)
                    .FirstOrDefaultAsync(x => x.Id == viewmodel.Id);

                if (existing == null)
                    return NotFound();

                existing.Name = viewmodel.Name;
                existing.CategoryId = viewmodel.SelectedCategory;

                // Clear old links safely
                dbContext.ItemLinks.RemoveRange(existing.DismantleIntoLinks);

                // Add new DismantleInto links
                foreach (var i in viewmodel.SelectedDismantleIntos)
                {
                    existing.DismantleIntoLinks.Add(new ItemLink
                    {
                        ResultItemId = i
                    });
                }

                
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            var item = await dbContext.Items
        .Include(i => i.DismantleIntoLinks)
        .FirstOrDefaultAsync(i => i.Id == id);

            if (item != null)
            {
                // Remove related ItemLinks first
                dbContext.ItemLinks.RemoveRange(item.DismantleIntoLinks);

                dbContext.Items.Remove(item);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
