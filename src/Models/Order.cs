using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models;

public class Order
{
    public enum OrderStatus
    {
        Pending,
        Payed,
        Cancelled
    }

    [Key]
    public int OrderId { set; get; }

    [Required]
    public int CustomerId { set; get; }

    public OrderStatus Status = OrderStatus.Pending;

    [Required]
    public decimal TotalAmount;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;

    public List<OrderItem> OrderItems { get; set; } = new();
}