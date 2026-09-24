using LibraryApi.Models;
namespace LibraryApi.Repositories;

public interface IFineRepository
{
    Task<List<Fine>> GetAllAsync(int? memberId, string? status);
    Task<Fine?> GetByIdAsync(int id);          // بيرجع الغرامة ومعاها الدفعات
    Task AddAsync(Fine fine);
    void Remove(Fine fine);
    Task<bool> MemberExistsAsync(int memberId);
    Task<bool> LoanBelongsToMemberAsync(int loanId, int memberId);
    Task SaveChangesAsync();
}
