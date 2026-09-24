using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Loans
{
    public class CreateLoanDto
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int BookCopyId { get; set; }
    }
}