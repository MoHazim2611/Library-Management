using LibraryManagement.API.Models;
using WebApplication1.Models;

namespace LibraryManagement.API.DTOs.Loans
{
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; }
        public int RenewalCount { get; set; }
    }
}