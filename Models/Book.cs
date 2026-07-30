using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    // Full entity as stored in the database.
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(150)]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(20)]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

        // Security concern from the spec: IsAvailable must never be
        // set directly from user input on Create/Edit forms.
        [Display(Name = "Availability")]
        public bool IsAvailable { get; set; } = true;
    }
}
