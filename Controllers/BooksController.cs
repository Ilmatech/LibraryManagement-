using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books  -> Home / Index / Listing Page
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.AsNoTracking().ToListAsync();
            return View(books);
        }

        // GET: Books/Details/5 -> Book Details Page
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return RedirectToNotFoundPage(id.Value);
            }

            return View(book);
        }

        // GET: Books/Create -> Create Book Page
        public IActionResult Create()
        {
            return View(new BookFormViewModel());
        }

        // POST: Books/Create
        // Binds ONLY to BookFormViewModel, which has no BookId or
        // IsAvailable property, so those fields can never be set
        // by a malicious or malformed form post.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var book = new Book
            {
                Title = form.Title,
                Author = form.Author,
                ISBN = form.ISBN,
                PublishedDate = form.PublishedDate,
                IsAvailable = true // always true for a brand-new book
            };

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Edit/5 -> Edit Book Page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return RedirectToNotFoundPage(id.Value);
            }

            var form = new BookFormViewModel
            {
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate
            };

            ViewData["BookId"] = book.BookId;
            return View(form);
        }

        // POST: Books/Edit/5
        // Only Title, Author, ISBN and PublishedDate can be changed here.
        // BookId comes from the route, not the form body, and IsAvailable
        // is loaded fresh from the database and never overwritten by input.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookFormViewModel form)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return RedirectToNotFoundPage(id);
            }

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = id;
                return View(form);
            }

            book.Title = form.Title;
            book.Author = form.Author;
            book.ISBN = form.ISBN;
            book.PublishedDate = form.PublishedDate;
            // book.IsAvailable is intentionally left untouched.

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5 -> Delete Book Page (confirmation)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return RedirectToNotFoundPage(id.Value);
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper: redirect to the custom Not Found page with the id
        // that could not be located, e.g. /Books/NotFoundPage/11
        private IActionResult RedirectToNotFoundPage(int id)
        {
            return RedirectToAction("NotFoundPage", new { id });
        }

        // GET: Books/NotFoundPage/11 -> "Resource Not Found" page
        public IActionResult NotFoundPage(int id)
        {
            ViewData["BookId"] = id;
            return View();
        }
    }
}
