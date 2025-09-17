using System.ComponentModel.DataAnnotations;
using OrderManagement.Models;

namespace OrderManagement.DTOs;

public record OrderItemDTO(
    string ProductName,
    int Quantity,
    decimal Price
);
public record OrderDTO(
    int OrderId,
    int CustomerId,
    decimal TotalAmount,
    [Required]
    List<OrderItemDTO> OrderItems
);

public class CreateOrderItem
{
    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public decimal Price { get; set; }
}
public class CreateOrderDTO
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public List<CreateOrderItem> OrderItems { get; set; } = new();
}

public class UpdateOrderDTO
{
    [Required]
    public int CustomerId { get; set; }

    public OrderStatus Status = OrderStatus.Pending;

    [Required]
    public List<CreateOrderItem> OrderItems { get; set; } = new();
}



