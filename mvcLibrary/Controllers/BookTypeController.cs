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
    public class BookTypeController : Controller
    {
        private readonly LibraryDbContext _context;

        public BookTypeController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: BookType
        public async Task<IActionResult> Index(string searchValue)
        {
            // Fetch all bookings including related Venue and Event data
            var bookType = await _context.BookType.ToListAsync();

            // Proceed if either searchBy or searchValue is not null/empty
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                // If searchValue is empty or whitespace, return full booking list
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    return View(bookType);
                }
                else
                {
                    bookType = bookType.Where(b => b.TypeID.ToString() == searchValue || b.Type.ToLower().Contains(searchValue)).ToList() ;
                }

            }

            // If no search parameters are provided, return all bookings
            return View(bookType);
        
        }

        // GET: BookType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (bookType == null)
            {
                return NotFound();
            }

            return View(bookType);
        }

        // GET: BookType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeID,Type")] BookType bookType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookType);
        }

        // GET: BookType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType.FindAsync(id);
            if (bookType == null)
            {
                return NotFound();
            }
            return View(bookType);
        }

        // POST: BookType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeID,Type")] BookType bookType)
        {
            if (id != bookType.TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookTypeExists(bookType.TypeID))
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
            return View(bookType);
        }

        // GET: BookType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (bookType == null)
            {
                return NotFound();
            }

            return View(bookType);
        }

        // POST: BookType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookType = await _context.BookType.FindAsync(id);
            if (bookType != null)
            {
                _context.BookType.Remove(bookType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookTypeExists(int id)
        {
            return _context.BookType.Any(e => e.TypeID == id);
        }
    }
}
