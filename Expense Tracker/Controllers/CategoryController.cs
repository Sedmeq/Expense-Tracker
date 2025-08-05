using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            try
            {
                if (_context.Categories == null)
                {
                    _logger.LogError("Categories DbSet is null");
                    return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
                }

                var categories = await _context.Categories
                    .OrderBy(c => c.Type)
                    .ThenBy(c => c.Title)
                    .ToListAsync();

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading categories");
                return Problem("An error occurred while loading categories.");
            }
        }

        // GET: Category/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    return View(new Category());
                }
                else
                {
                    var category = await _context.Categories.FindAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading category for editing");
                return Problem("An error occurred while loading the category.");
            }
        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check for duplicate titles
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Title.ToLower() == category.Title.ToLower()
                                                 && c.CategoryId != category.CategoryId);

                    if (existingCategory != null)
                    {
                        ModelState.AddModelError("Title", "A category with this title already exists.");
                        return View(category);
                    }

                    if (category.CategoryId == 0)
                    {
                        _context.Add(category);
                        _logger.LogInformation("Creating new category: {Title}", category.Title);
                    }
                    else
                    {
                        _context.Update(category);
                        _logger.LogInformation("Updating category: {Title}", category.Title);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving category");
                ModelState.AddModelError("", "An error occurred while saving the category.");
                return View(category);
            }
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Categories == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
                }

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                // Check if category has associated transactions
                var hasTransactions = await _context.Transactions
                    .AnyAsync(t => t.CategoryId == id);

                if (hasTransactions)
                {
                    TempData["ErrorMessage"] = "Cannot delete this category because it has associated transactions.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted category: {Title}", category.Title);
                TempData["SuccessMessage"] = "Category deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category");
                TempData["ErrorMessage"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}