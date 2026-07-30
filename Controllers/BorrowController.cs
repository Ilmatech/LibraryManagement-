using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibraryContext _context;

        public BorrowController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Borrow/Create?bookId=1
        // This is the action that was missing, which caused the
        // "This localhost page can't be found" 404 in the PDF.
        public async Task<IActionResult> Create(int bookId)
        {
            var book = await _context.Books.AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                return RedirectToAction("NotFoundPage", "Books", new { id = bookId });
            }

            if (!book.IsAvailable)
            {
                TempData["Error"] = "This book is already borrowed.";
                return RedirectToAction("Index", "Books");
            }

            var model = new BorrowRecord
            {
                BookId = book.BookId,
                Book = book,
                BorrowDate = DateTime.Today
            };

            return View(model);
        }

        // POST: Borrow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowRecord form)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == form.BookId);
            if (book == null)
            {
                return RedirectToAction("NotFoundPage", "Books", new { id = form.BookId });
            }

            if (!ModelState.IsValid)
            {
                form.Book = book;
                return View(form);
            }

            var record = new BorrowRecord
            {
                BookId = book.BookId,
                BorrowerName = form.BorrowerName,
                BorrowDate = form.BorrowDate == default ? DateTime.Today : form.BorrowDate
            };

            book.IsAvailable = false; // the only place IsAvailable changes for a borrow

            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Books");
        }

        // POST: Borrow/Return/5  (BorrowRecordId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var record = await _context.BorrowRecords.FirstOrDefaultAsync(r => r.BorrowRecordId == id);
            if (record == null || record.ReturnDate != null)
            {
                return RedirectToAction("Index", "Books");
            }

            record.ReturnDate = DateTime.Today;

            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == record.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Books");
        }
    }
}
