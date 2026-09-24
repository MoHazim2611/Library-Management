namespace LibraryApi.Models;

public class Payment
{
    public int Id { get; set; }
    public int FineId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }

    public Fine? Fine { get; set; }
}
