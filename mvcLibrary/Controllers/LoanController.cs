using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcLibrary.Models;

namespace mvcLibrary.Controllers
{
    public class LoanController : Controller
    {
        private readonly LibraryDbContext _context;

        public LoanController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Loan
        public async Task<IActionResult> Index(string searchValue)
        {
            // Fetch all bookings including related Venue and Event data
            var loan = from b in _context.Loan.Include(b => b.Book)
                       select b;

            // Proceed if either searchBy or searchValue is not null/empty
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                // If searchValue is empty or whitespace, return full booking list
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    return View(loan);
                }
                else
                {
                    loan = loan.Where(b =>
                        b.LoanID.ToString() == searchValue ||
                        (b.Book.Title != null && b.Book.Title.ToLower().Contains(searchValue))); 
                }

            }

            // If no search parameters are provided, return all bookings
            return View(loan);
        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loan/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID");
            return View();
        }

        // POST: Loan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanID,LoanDate,BookID")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanID,LoanDate,BookID")] Loan loan)
        {
            if (id != loan.LoanID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanID))
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
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // GET: Loan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loan.FindAsync(id);
            if (loan != null)
            {
                _context.Loan.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loan.Any(e => e.LoanID == id);
        }
    }
}
