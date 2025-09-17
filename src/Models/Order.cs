using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models;

public enum OrderStatus
{
    Pending,
    Payed,
    Cancelled
}
public class Order
{

    [Key]
    public int OrderId { set; get; }

    [Required]
    public int CustomerId { set; get; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required]
    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;

    public List<OrderItem> OrderItems { get; set; } = new();
}