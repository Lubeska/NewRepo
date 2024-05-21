using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Books.Data;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Books.Controllers
{
    [Authorize]
    public class MyBooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyBooksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MyBooks
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var myBooks = await _context.MyBooks
                .Include(mb => mb.Book)
                .ThenInclude(b => b.Author)
                .Where(mb => mb.UserId == userId)
                .ToListAsync();
            
            return View(myBooks);
        }

        // POST: MyBooks/Buy/5
        [HttpPost]
        public async Task<IActionResult> Buy(int bookId)
        {
            var userId = _userManager.GetUserId(User);

            if (!_context.MyBooks.Any(mb => mb.BookId == bookId && mb.UserId == userId))
            {
                var myBook = new MyBooks
                {
                    UserId = userId,
                    BookId = bookId
                };

                _context.MyBooks.Add(myBook);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: MyBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks
                .Include(mb => mb.Book)
                .ThenInclude(b => b.Author)
                .Include(mb => mb.Book)
                .ThenInclude(b => b.BookGenre)
                .ThenInclude(bg => bg.Genre)
                .Include(mb => mb.Book)
                .ThenInclude(b => b.Reviews)
                .FirstOrDefaultAsync(mb => mb.Id == id);

            if (myBooks == null)
            {
                return NotFound();
            }

            return View(myBooks);
        }

        // GET: MyBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title");
            return View();
        }

        // POST: MyBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,BookId")] MyBooks myBooks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myBooks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title", myBooks.BookId);
            return View(myBooks);
        }

        // GET: MyBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks.FindAsync(id);
            if (myBooks == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title", myBooks.BookId);
            return View(myBooks);
        }

        // POST: MyBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BookId")] MyBooks myBooks)
        {
            if (id != myBooks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myBooks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyBooksExists(myBooks.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title", myBooks.BookId);
            return View(myBooks);
        }

        // GET: MyBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks
                .Include(m => m.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myBooks == null)
            {
                return NotFound();
            }

            return View(myBooks);
        }

        // POST: MyBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myBooks = await _context.MyBooks.FindAsync(id);
            if (myBooks != null)
            {
                _context.MyBooks.Remove(myBooks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyBooksExists(int id)
        {
            return _context.MyBooks.Any(e => e.Id == id);
        }
    }
}
