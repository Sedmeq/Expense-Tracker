using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                // Check if context is available
                if (_context?.Transactions == null || _context?.Categories == null)
                {
                    _logger.LogError("Database context or tables are not available");
                    ViewBag.ErrorMessage = "Database connection issue. Please try again later.";
                    return View();
                }

                // Last 7 Days
                DateTime startDate = DateTime.Today.AddDays(-6);
                DateTime endDate = DateTime.Today;

                var selectedTransactions = await _context.Transactions
                    .Include(x => x.Category)
                    .Where(y => y.Date >= startDate && y.Date <= endDate)
                    .ToListAsync();

                // Calculate totals with improved null checking
                int totalIncome = selectedTransactions
                    .Where(i => i.Category != null &&
                               string.Equals(i.Category.Type, "Income", StringComparison.OrdinalIgnoreCase))
                    .Sum(j => j.Amount);

                int totalExpense = selectedTransactions
                    .Where(i => i.Category != null &&
                               string.Equals(i.Category.Type, "Expense", StringComparison.OrdinalIgnoreCase))
                    .Sum(j => j.Amount);

                int balance = totalIncome - totalExpense;

                // Format currency values safely
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;

                ViewBag.TotalIncome = totalIncome.ToString("C0", culture);
                ViewBag.TotalExpense = totalExpense.ToString("C0", culture);
                ViewBag.Balance = balance.ToString("C0", culture);

                // Doughnut Chart - Expense By Category with better error handling
                var expenseData = selectedTransactions
                    .Where(i => i.Category != null &&
                               string.Equals(i.Category.Type, "Expense", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(j => j.Category!.CategoryId)
                    .Select(k => new
                    {
                        categoryTitleWithIcon = k.First().Category?.TitleWithIcon ?? "Unknown",
                        amount = k.Sum(j => j.Amount),
                        formattedAmount = k.Sum(j => j.Amount).ToString("C0", culture),
                    })
                    .OrderByDescending(l => l.amount)
                    .ToList();

                ViewBag.DoughnutChartData = expenseData;

                // Spline Chart - Income vs Expense with better data handling
                var incomeSummary = selectedTransactions
                    .Where(i => i.Category != null &&
                               string.Equals(i.Category.Type, "Income", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(j => j.Date)
                    .Select(k => new SplineChartData
                    {
                        day = k.First().Date.ToString("dd-MMM"),
                        income = k.Sum(l => l.Amount),
                        expense = 0
                    })
                    .ToList();

                var expenseSummary = selectedTransactions
                    .Where(i => i.Category != null &&
                               string.Equals(i.Category.Type, "Expense", StringComparison.OrdinalIgnoreCase))
                    .GroupBy(j => j.Date)
                    .Select(k => new SplineChartData
                    {
                        day = k.First().Date.ToString("dd-MMM"),
                        income = 0,
                        expense = k.Sum(l => l.Amount)
                    })
                    .ToList();

                // Combine Income & Expense for all 7 days
                string[] last7Days = Enumerable.Range(0, 7)
                    .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
                    .ToArray();

                ViewBag.SplineChartData = from day in last7Days
                                          join income in incomeSummary on day equals income.day into dayIncomeJoined
                                          from income in dayIncomeJoined.DefaultIfEmpty()
                                          join expense in expenseSummary on day equals expense.day into expenseJoined
                                          from expense in expenseJoined.DefaultIfEmpty()
                                          select new
                                          {
                                              day = day,
                                              income = income?.income ?? 0,
                                              expense = expense?.expense ?? 0,
                                          };

                // Recent Transactions with safer query
                var recentTransactions = await _context.Transactions
                    .Include(i => i.Category)
                    .OrderByDescending(j => j.Date)
                    .ThenByDescending(j => j.TransactionId)
                    .Take(5)
                    .ToListAsync();

                ViewBag.RecentTransactions = recentTransactions ?? new List<Transaction>();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading dashboard data");
                ViewBag.ErrorMessage = "Unable to load dashboard data. Please try again later.";

                // Set default values to prevent view errors
                ViewBag.TotalIncome = "$0";
                ViewBag.TotalExpense = "$0";
                ViewBag.Balance = "$0";
                ViewBag.DoughnutChartData = new List<object>();
                ViewBag.SplineChartData = new List<object>();
                ViewBag.RecentTransactions = new List<Transaction>();

                return View();
            }
        }
    }

    public class SplineChartData
    {
        public string day { get; set; } = "";
        public int income { get; set; }
        public int expense { get; set; }
    }
}