using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos;

public class CreatePaymentDto
{
    [Required] public int FineId { get; set; }
    [Range(0.01, 99999999.99)] public decimal Amount { get; set; }

    [Required, RegularExpression("^(Cash|Card|Online)$",
        ErrorMessage = "PaymentMethod must be Cash, Card or Online")]
    public string PaymentMethod { get; set; } = string.Empty;
}
