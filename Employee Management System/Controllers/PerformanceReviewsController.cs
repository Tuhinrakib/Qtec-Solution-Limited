using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Management_System.Models;

namespace Employee_Management_System.Controllers
{
    public class PerformanceReviewsController : Controller
    {
        private readonly EmployeeDbContext _context;

        public PerformanceReviewsController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: PerformanceReviews
        public async Task<IActionResult> Index()
        {
            var employeeDbContext = _context.PerformanceReviews.Include(p => p.Employee);
            return View(await employeeDbContext.ToListAsync());
        }

        // GET: PerformanceReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PerformanceReviews == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (performanceReview == null)
            {
                return NotFound();
            }

            return View(performanceReview);
        }

        // GET: PerformanceReviews/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Email");
            return View();
        }

        // POST: PerformanceReviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,EmployeeID,ReviewDate,ReviewScore,ReviewNotes")] PerformanceReview performanceReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performanceReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Email", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // GET: PerformanceReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PerformanceReviews == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            if (performanceReview == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Email", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // POST: PerformanceReviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,EmployeeID,ReviewDate,ReviewScore,ReviewNotes")] PerformanceReview performanceReview)
        {
            if (id != performanceReview.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performanceReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceReviewExists(performanceReview.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "Email", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // GET: PerformanceReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PerformanceReviews == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (performanceReview == null)
            {
                return NotFound();
            }

            return View(performanceReview);
        }

        // POST: PerformanceReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PerformanceReviews == null)
            {
                return Problem("Entity set 'EmployeeDbContext.PerformanceReviews'  is null.");
            }
            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            if (performanceReview != null)
            {
                _context.PerformanceReviews.Remove(performanceReview);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceReviewExists(int id)
        {
          return (_context.PerformanceReviews?.Any(e => e.ReviewID == id)).GetValueOrDefault();
        }
    }
}
