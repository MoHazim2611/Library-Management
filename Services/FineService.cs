using LibraryApi.Common;
using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services;

public class FineService : IFineService
{
    private readonly IFineRepository _fines;
    public FineService(IFineRepository fines) => _fines = fines;

    public async Task<ServiceResult<List<FineDto>>> GetAllAsync(int? memberId, string? status)
    {
        if (!string.IsNullOrWhiteSpace(status) && !FineStatus.All.Contains(status))
            return ServiceResult<List<FineDto>>.Invalid(
                $"Invalid status. Allowed: {string.Join(", ", FineStatus.All)}");

        var list = await _fines.GetAllAsync(memberId, status);
        return ServiceResult<List<FineDto>>.Ok(list.Select(ToDto).ToList());
    }

    public async Task<ServiceResult<FineDetailsDto>> GetByIdAsync(int id)
    {
        var f = await _fines.GetByIdAsync(id);
        if (f is null) return ServiceResult<FineDetailsDto>.NotFound($"Fine {id} not found");

        var dto = new FineDetailsDto(
            f.Id, f.LoanId, f.MemberId, f.Amount, f.Reason, f.Status, f.CreatedAt,
            f.PaidAmount, f.RemainingAmount,
            f.Payments.OrderBy(p => p.PaidAt)
                .Select(p => new PaymentDto(p.Id, p.FineId, p.Amount, p.PaymentMethod, p.PaidAt)).ToList());
        return ServiceResult<FineDetailsDto>.Ok(dto);
    }

    public async Task<ServiceResult<MemberFinesSummaryDto>> GetMemberSummaryAsync(int memberId)
    {
        if (!await _fines.MemberExistsAsync(memberId))
            return ServiceResult<MemberFinesSummaryDto>.NotFound($"Member {memberId} not found");

        var fines = await _fines.GetAllAsync(memberId, null);
        var total = fines.Sum(f => f.Amount);
        var paid = fines.Sum(f => f.PaidAmount);
        return ServiceResult<MemberFinesSummaryDto>.Ok(
            new MemberFinesSummaryDto(memberId, fines.Count, total, paid, total - paid));
    }

    public async Task<ServiceResult<FineDto>> CreateAsync(CreateFineDto dto)
    {
        if (!await _fines.MemberExistsAsync(dto.MemberId))
            return ServiceResult<FineDto>.NotFound($"Member {dto.MemberId} not found");

        // الـ Loan لازم يكون موجود وبتاع نفس العضو
        if (!await _fines.LoanBelongsToMemberAsync(dto.LoanId, dto.MemberId))
            return ServiceResult<FineDto>.Invalid(
                $"Loan {dto.LoanId} does not exist or does not belong to member {dto.MemberId}");

        var fine = new Fine
        {
            LoanId = dto.LoanId,
            MemberId = dto.MemberId,
            Amount = dto.Amount,
            Reason = dto.Reason.Trim(),
            Status = FineStatus.Unpaid
        };

        await _fines.AddAsync(fine);
        await _fines.SaveChangesAsync();
        return ServiceResult<FineDto>.Ok(ToDto(fine));
    }

    public async Task<ServiceResult<bool>> UpdateAsync(int id, UpdateFineDto dto)
    {
        var fine = await _fines.GetByIdAsync(id);
        if (fine is null) return ServiceResult<bool>.NotFound($"Fine {id} not found");

        if (dto.Amount < fine.PaidAmount)
            return ServiceResult<bool>.Invalid(
                $"Amount cannot be less than the already paid amount ({fine.PaidAmount})");

        fine.Amount = dto.Amount;
        fine.Reason = dto.Reason.Trim();
        fine.RecalculateStatus();

        await _fines.SaveChangesAsync();
        return ServiceResult<bool>.Ok(true);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var fine = await _fines.GetByIdAsync(id);
        if (fine is null) return ServiceResult<bool>.NotFound($"Fine {id} not found");
        if (fine.Payments.Any())
            return ServiceResult<bool>.Conflict("Cannot delete a fine that already has payments");

        _fines.Remove(fine);
        await _fines.SaveChangesAsync();
        return ServiceResult<bool>.Ok(true);
    }

    private static FineDto ToDto(Fine f) => new(
        f.Id, f.LoanId, f.MemberId, f.Amount, f.Reason, f.Status, f.CreatedAt,
        f.PaidAmount, f.RemainingAmount);
}
