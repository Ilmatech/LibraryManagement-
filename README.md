# LibraryManagement

An ASP.NET Core MVC + EF Core Library Management System, built to match the
Project Session 1 requirements: administrators can list, view, add, edit,
delete, and borrow/return books, with `BookId` and `IsAvailable` protected
from direct user input.

## Project structure

```
LibraryManagement/
├── Controllers/
│   ├── BooksController.cs     # Index, Details, Create, Edit, Delete, NotFoundPage
│   └── BorrowController.cs    # Create (borrow), Return  -> fixes the 404 from the PDF
├── Models/
│   ├── Book.cs                 # Full entity (BookId, Title, Author, ISBN, PublishedDate, IsAvailable)
│   ├── BookFormViewModel.cs    # Create/Edit form model - no BookId/IsAvailable, so they can't be tampered with
│   └── BorrowRecord.cs
├── Data/
│   └── LibraryContext.cs       # DbContext + seed data (matches the 4 books in the PDF screenshots)
├── Views/
│   ├── Books/  (Index, Details, Create, Edit, Delete, NotFoundPage)
│   ├── Borrow/ (Create)
│   └── Shared/ (_Layout, _ValidationScriptsPartial)
├── Program.cs
└── appsettings.json
```

## How to run (Visual Studio / .NET 8 SDK)

1. Open `LibraryManagement.csproj` in Visual Studio 2022 (or run from CLI).
2. Restore NuGet packages (EF Core SqlServer/Tools/Design are already referenced).
3. Update the connection string in `appsettings.json` if needed (defaults to LocalDB).
4. Open the Package Manager Console and run, exactly as in the PDF:
   ```
   Add-Migration Mig1
   Update-Database
   ```
   This creates the `LibraryManagementDB` database with the `Books` and
   `BorrowRecords` tables, and seeds the 4 sample books.
5. Run the project (F5). It opens directly to `Books/Index` (the default
   route, same as `Program.cs`: `{controller=Books}/{action=Index}/{id?}`).

## What was fixed from the PDF

The notes ended with a 404 at `Borrow/Create?bookId=1` because no
`BorrowController` existed yet. This project adds:
- `BorrowController.Create(int bookId)` — GET, shows the borrow form
- `BorrowController.Create(BorrowRecord form)` — POST, saves the borrow record and flips `IsAvailable` to false
- `BorrowController.Return(int id)` — POST, marks a book available again

Clicking **Borrow** on the Books List now works end-to-end instead of 404ing.
