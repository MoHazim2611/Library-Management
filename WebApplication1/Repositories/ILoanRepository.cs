using LibraryManagement.API.Models;
using WebApplication1.Models;

namespace LibraryManagement.API.Repositories
{
    public interface ILoanRepository
    {
        Task<Loan?> GetByIdAsync(int id);
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId);
        Task<int> GetActiveLoansCountByMemberIdAsync(int memberId);
        Task AddAsync(Loan loan);
        Task UpdateAsync(Loan loan);
        Task SaveChangesAsync();
    }
}