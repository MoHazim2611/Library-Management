using System;

namespace WebApplication1.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; } = LoanStatus.Active;
        public int RenewalCount { get; set; } = 0;
    }
}