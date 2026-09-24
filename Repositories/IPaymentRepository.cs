
using LibraryApi.Models;
namespace LibraryApi.Repositories;

public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync(int? fineId);
    Task<Payment?> GetByIdAsync(int id);
    Task AddAsync(Payment payment);
}
