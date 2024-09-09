using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PaymentSummaryDto
    {
        [Required] public int Last4 { get; set; }
        [Required] public string Brand { get; set; } = string.Empty;
        [Required] public int ExpMonth { get; set; }
        [Required] public int ExpYear { get; set; }
    }
}
