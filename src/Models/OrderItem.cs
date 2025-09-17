using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { set; get; }

    [Required]
    public int OrderId { set; get; }

    [Required]
    public string ProductName { set; get; } = null!;

    [Required]
    public int Quantity { set; get; }

    [Required]
    public decimal Price { set; get; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = null!;
}