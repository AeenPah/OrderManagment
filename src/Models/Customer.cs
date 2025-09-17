using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models;

public class Customer
{
    [Key]
    public int CustomerId { set; get; }

    [Required]
    public string Username { set; get; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public byte[] Salt { get; set; } = null!;

    [Required]
    public string Email { set; get; } = null!;

    public decimal WalletBalance { set; get; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}