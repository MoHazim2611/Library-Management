using LibraryManagement.API.DTOs.Loans;
using LibraryManagement.API.Models;
using LibraryManagement.API.Repositories;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace LibraryManagement.API.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IConfiguration _configuration;

        public LoanService(ILoanRepository loanRepository, IConfiguration configuration)
        {
            _loanRepository = loanRepository;
            _configuration = configuration;
        }

        public async Task<LoanResponseDto> CreateLoanAsync(CreateLoanDto dto)
        {
            int maxConcurrentLoans = _configuration.GetValue<int>("LoanSettings:MaxConcurrentLoans", 5);
            int loanPeriodDays = _configuration.GetValue<int>("LoanSettings:LoanPeriodDays", 14);

            int currentActiveLoans = await _loanRepository.GetActiveLoansCountByMemberIdAsync(dto.MemberId);
            if (currentActiveLoans >= maxConcurrentLoans)
            {
                throw new InvalidOperationException($"العضو وصل للحد الأقصى للاستعارات المسموحة ({maxConcurrentLoans}).");
            }

            var loan = new Loan
            {
                MemberId = dto.MemberId,
                BookCopyId = dto.BookCopyId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(loanPeriodDays),
                Status = LoanStatus.Active,
                RenewalCount = 0
            };

            await _loanRepository.AddAsync(loan);
            await _loanRepository.SaveChangesAsync();

            return MapToResponseDto(loan);
        }

        public async Task<LoanResponseDto?> ReturnLoanAsync(int loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return null;

            loan.ReturnDate = DateTime.UtcNow;
            loan.Status = LoanStatus.Returned;

            await _loanRepository.UpdateAsync(loan);
            await _loanRepository.SaveChangesAsync();

            return MapToResponseDto(loan);
        }

        public async Task<LoanResponseDto?> RenewLoanAsync(int loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return null;

            int maxRenewals = _configuration.GetValue<int>("LoanSettings:MaxRenewals", 2);
            int loanPeriodDays = _configuration.GetValue<int>("LoanSettings:LoanPeriodDays", 14);

            if (loan.RenewalCount >= maxRenewals)
            {
                throw new InvalidOperationException($"تم الوصول للحد الأقصى لمرات التجديد ({maxRenewals}).");
            }

            if (loan.Status != LoanStatus.Active)
            {
                throw new InvalidOperationException("لا يمكن تجديد استعارة غير نشطة.");
            }

            loan.RenewalCount++;
            loan.DueDate = loan.DueDate.AddDays(loanPeriodDays);

            await _loanRepository.UpdateAsync(loan);
            await _loanRepository.SaveChangesAsync();

            return MapToResponseDto(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync()
        {
            var loans = await _loanRepository.GetAllAsync();
            return loans.Select(MapToResponseDto);
        }

        public async Task<LoanResponseDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            return loan == null ? null : MapToResponseDto(loan);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetOverdueLoansAsync()
        {
            var loans = await _loanRepository.GetOverdueLoansAsync();
            return loans.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<LoanResponseDto>> GetActiveLoansByMemberAsync(int memberId)
        {
            var loans = await _loanRepository.GetActiveLoansByMemberIdAsync(memberId);
            return loans.Select(MapToResponseDto);
        }

        private static LoanResponseDto MapToResponseDto(Loan loan)
        {
            return new LoanResponseDto
            {
                Id = loan.Id,
                MemberId = loan.MemberId,
                BookCopyId = loan.BookCopyId,
                BorrowDate = loan.BorrowDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = loan.Status,
                RenewalCount = loan.RenewalCount
            };
        }
    }
}