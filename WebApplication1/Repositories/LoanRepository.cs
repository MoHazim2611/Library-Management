using LibraryManagement.API.Data;
using LibraryManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace LibraryManagement.API.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans.ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans
                .Where(l => l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId)
        {
            return await _context.Loans
                .Where(l => l.MemberId == memberId && l.Status == LoanStatus.Active)
                .ToListAsync();
        }

        public async Task<int> GetActiveLoansCountByMemberIdAsync(int memberId)
        {
            return await _context.Loans
                .CountAsync(l => l.MemberId == memberId && l.Status == LoanStatus.Active);
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}