using LibraryApi.Common;
using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _payments;
    private readonly IFineRepository _fines;

    public PaymentService(IPaymentRepository payments, IFineRepository fines)
    {
        _payments = payments;
        _fines = fines;
    }

    public async Task<ServiceResult<List<PaymentDto>>> GetAllAsync(int? fineId)
    {
        var list = await _payments.GetAllAsync(fineId);
        return ServiceResult<List<PaymentDto>>.Ok(list.Select(ToDto).ToList());
    }

    public async Task<ServiceResult<PaymentDto>> GetByIdAsync(int id)
    {
        var p = await _payments.GetByIdAsync(id);
        return p is null
            ? ServiceResult<PaymentDto>.NotFound($"Payment {id} not found")
            : ServiceResult<PaymentDto>.Ok(ToDto(p));
    }

    public async Task<ServiceResult<PaymentDto>> CreateAsync(CreatePaymentDto dto)
    {
        var fine = await _fines.GetByIdAsync(dto.FineId);
        if (fine is null) return ServiceResult<PaymentDto>.NotFound($"Fine {dto.FineId} not found");

        if (fine.RemainingAmount <= 0)
            return ServiceResult<PaymentDto>.Conflict("This fine is already fully paid");

        if (dto.Amount > fine.RemainingAmount)
            return ServiceResult<PaymentDto>.Invalid(
                $"Payment exceeds the remaining amount ({fine.RemainingAmount})");

        var payment = new Payment
        {
            FineId = fine.Id,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod
        };

        await _payments.AddAsync(payment);
        fine.Payments.Add(payment);
        fine.RecalculateStatus();          // Unpaid -> PartiallyPaid -> Paid

        // Save واحد = transaction واحدة: الدفعة + تحديث حالة الغرامة
        await _fines.SaveChangesAsync();
        return ServiceResult<PaymentDto>.Ok(ToDto(payment));
    }

    private static PaymentDto ToDto(Payment p) =>
        new(p.Id, p.FineId, p.Amount, p.PaymentMethod, p.PaidAt);
}
