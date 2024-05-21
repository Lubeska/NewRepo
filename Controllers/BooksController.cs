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
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        
        // GET: Books
        public async Task<IActionResult> Index(string authorName, string bookGenre, string searchString)
        {
            var booksQuery = _context.Book
        .Include(b => b.Author)
        .Include(b => b.BookGenre)
        .ThenInclude(bg => bg.Genre)
        .AsQueryable();

            if (!string.IsNullOrEmpty(authorName))
            {
                var lowerAuthorName = authorName.ToLower();
                booksQuery = booksQuery.Where(b => b.Author.FirstName.ToLower().Contains(lowerAuthorName) ||
                                                    b.Author.LastName.ToLower().Contains(lowerAuthorName));
                ViewData["AuthorFilter"] = authorName;
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                var lowerBookGenre = bookGenre.ToLower();
                booksQuery = booksQuery.Where(b => b.BookGenre.Any(bg => bg.Genre.GenreName.ToLower().Contains(lowerBookGenre)));
                ViewData["GenreFilter"] = bookGenre;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearchString = searchString.ToLower();
                booksQuery = booksQuery.Where(b => b.Title.ToLower().Contains(lowerSearchString));
                ViewData["TitleFilter"] = searchString;
            }

            var books = await booksQuery.ToListAsync();

            return View(books);
        }

       
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.BookGenre)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var reviews = await _context.Review
                .Where(r => r.BookID == id)
                .ToListAsync();

            ViewData["Reviews"] = reviews;

            return View(book);
        }

        // POST: Books/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(int id, [Bind("BookID, AppUser, Comment, Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.BookID = id;

                review.AppUser = User.Identity.Name;

                _context.Add(review);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YearPublished,NumPages,Description,Publisher,DownloadUrl,AuthorId")] Book book, IFormFile frontPage)
        {
            if (ModelState.IsValid)
            {
                if (frontPage != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(frontPage.FileName);
                    string fileDir = Path.Combine(wwwRootPath, @"images");

                    if (!Directory.Exists(fileDir))
                        Directory.CreateDirectory(fileDir);

                    using (var fileStream = new FileStream(Path.Combine(fileDir, fileName), FileMode.Create))
                    {
                        await frontPage.CopyToAsync(fileStream);
                    }

                    book.FrontPage = @"/images/" + fileName;
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,YearPublished,NumPages,Description,Publisher,FrontPage,DownloadUrl,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FirstName", book.AuthorId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (!_context.MyBooks.Any(mb => mb.BookId == id && mb.UserId == userId))
            {
                var myBook = new MyBooks
                {
                    UserId = userId,
                    BookId = id
                };

                _context.MyBooks.Add(myBook);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "MyBooks");
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
