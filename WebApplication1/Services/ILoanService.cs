using LibraryManagement.API.DTOs.Loans;

namespace LibraryManagement.API.Services
{
    public interface ILoanService
    {
        Task<LoanResponseDto> CreateLoanAsync(CreateLoanDto dto);
        Task<LoanResponseDto?> ReturnLoanAsync(int loanId);
        Task<LoanResponseDto?> RenewLoanAsync(int loanId);
        Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync();
        Task<LoanResponseDto?> GetLoanByIdAsync(int id);
        Task<IEnumerable<LoanResponseDto>> GetOverdueLoansAsync();
        Task<IEnumerable<LoanResponseDto>> GetActiveLoansByMemberAsync(int memberId);
    }
}