using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class BorrowRecord
    {
        public int BorrowRecordId { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book? Book { get; set; }

        [Required(ErrorMessage = "Borrower name is required.")]
        [StringLength(150)]
        [Display(Name = "Borrower Name")]
        public string BorrowerName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Borrow Date")]
        public DateTime BorrowDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }
    }
}
