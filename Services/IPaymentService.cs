using LibraryApi.Common;
using LibraryApi.Dtos;

namespace LibraryApi.Services;

public interface IPaymentService
{
    Task<ServiceResult<List<PaymentDto>>> GetAllAsync(int? fineId);
    Task<ServiceResult<PaymentDto>> GetByIdAsync(int id);
    Task<ServiceResult<PaymentDto>> CreateAsync(CreatePaymentDto dto);
}
