using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos;

public class CreateFineDto
{
    [Required] public int LoanId { get; set; }
    [Required] public int MemberId { get; set; }
    [Range(0.01, 99999999.99)] public decimal Amount { get; set; }
    [Required, StringLength(250)] public string Reason { get; set; } = string.Empty;
}

public class UpdateFineDto
{
    [Range(0.01, 99999999.99)] public decimal Amount { get; set; }
    [Required, StringLength(250)] public string Reason { get; set; } = string.Empty;
}

public record PaymentDto(int Id, int FineId, decimal Amount, string PaymentMethod, DateTime PaidAt);

public record FineDto(
    int Id, int LoanId, int MemberId, decimal Amount, string Reason, string Status,
    DateTime CreatedAt, decimal PaidAmount, decimal RemainingAmount);

public record FineDetailsDto(
    int Id, int LoanId, int MemberId, decimal Amount, string Reason, string Status,
    DateTime CreatedAt, decimal PaidAmount, decimal RemainingAmount,
    List<PaymentDto> Payments);

public record MemberFinesSummaryDto(
    int MemberId, int FinesCount, decimal TotalFines, decimal TotalPaid, decimal TotalRemaining);
