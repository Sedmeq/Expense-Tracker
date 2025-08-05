using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ApplicationDbContext context, ILogger<TransactionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            try
            {
                var transactions = await _context.Transactions
                    .Include(t => t.Category)
                    .OrderByDescending(t => t.Date)
                    .ThenByDescending(t => t.TransactionId)
                    .ToListAsync();

                return View(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading transactions");
                return Problem("An error occurred while loading transactions.");
            }
        }

        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            try
            {
                await PopulateCategoriesAsync();

                if (id == 0)
                {
                    return View(new Transaction { Date = DateTime.Today });
                }
                else
                {
                    var transaction = await _context.Transactions.FindAsync(id);
                    if (transaction == null)
                    {
                        return NotFound();
                    }
                    return View(transaction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading transaction for editing");
                return Problem("An error occurred while loading the transaction.");
            }
        }

        // POST: Transaction/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Validate that the category exists
                    var categoryExists = await _context.Categories
                        .AnyAsync(c => c.CategoryId == transaction.CategoryId);

                    if (!categoryExists)
                    {
                        ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                        await PopulateCategoriesAsync();
                        return View(transaction);
                    }

                    // Validate date is not in the future
                    if (transaction.Date > DateTime.Today)
                    {
                        ModelState.AddModelError("Date", "Date cannot be in the future.");
                        await PopulateCategoriesAsync();
                        return View(transaction);
                    }

                    if (transaction.TransactionId == 0)
                    {
                        _context.Add(transaction);
                        _logger.LogInformation("Creating new transaction for amount: {Amount}", transaction.Amount);
                    }
                    else
                    {
                        _context.Update(transaction);
                        _logger.LogInformation("Updating transaction ID: {TransactionId}", transaction.TransactionId);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Transaction saved successfully.";
                    return RedirectToAction(nameof(Index));
                }

                await PopulateCategoriesAsync();
                return View(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving transaction");
                ModelState.AddModelError("", "An error occurred while saving the transaction.");
                await PopulateCategoriesAsync();
                return View(transaction);
            }
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Transactions == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Transactions' is null.");
                }

                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null)
                {
                    return NotFound();
                }

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted transaction ID: {TransactionId}", id);
                TempData["SuccessMessage"] = "Transaction deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting transaction");
                TempData["ErrorMessage"] = "An error occurred while deleting the transaction.";
                return RedirectToAction(nameof(Index));
            }
        }

        [NonAction]
        private async Task PopulateCategoriesAsync()
        {
            try
            {
                var categoryCollection = await _context.Categories
                    .OrderBy(c => c.Type)
                    .ThenBy(c => c.Title)
                    .ToListAsync();

                // Create default category object - DÜZELTME BURADA
                Category defaultCategory = new Category
                {
                    CategoryId = 0,
                    Title = "Choose a Category",
                    Icon = "", // Icon boş bırakıyoruz
                    Type = "Expense" // Default type
                };
                // TitleWithIcon property'sine değer atamaya gerek yok çünkü computed property

                categoryCollection.Insert(0, defaultCategory);
                ViewBag.Categories = categoryCollection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while populating categories");
                ViewBag.Categories = new List<Category>();
            }
        }
    }
}