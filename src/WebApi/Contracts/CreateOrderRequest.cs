using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts;

public sealed class CreateOrderRequest
{
    [Required]
    [StringLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal UnitPrice { get; set; }
}
