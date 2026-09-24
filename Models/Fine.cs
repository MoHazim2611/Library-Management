namespace LibraryApi.Models;

public static class FineStatus
{
    public const string Unpaid = "Unpaid";
    public const string PartiallyPaid = "PartiallyPaid";
    public const string Paid = "Paid";
    public static readonly string[] All = { Unpaid, PartiallyPaid, Paid };
}

public class Fine
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public int MemberId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = FineStatus.Unpaid;
    public DateTime CreatedAt { get; set; }

    public List<Payment> Payments { get; set; } = new();
    public decimal PaidAmount => Payments?.Sum(p => p.Amount) ?? 0;
    public decimal RemainingAmount => Amount - PaidAmount;
    public void RecalculateStatus()
    {
        if (PaidAmount == 0)
            Status = FineStatus.Unpaid;
        else if (PaidAmount >= Amount)
            Status = FineStatus.Paid;
        else
            Status = FineStatus.PartiallyPaid;
    }
}
