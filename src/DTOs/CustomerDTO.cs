using System.ComponentModel.DataAnnotations;

namespace OrderManagement.DTOs;

public record CustomerDTO(
    int CustomerId,
    string Username,
    string Email,
    decimal WalletBalance,
    DateTime CreatedAt
);

public class CreateCustomerDto
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Username { set; get; } = null!;

    [Required]
    [EmailAddress]
    public string Email { set; get; } = null!;

    [Required]
    [MinLength(4)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
        ErrorMessage = "Password must contain letters and numbers.")]
    public required string Password { set; get; } = null!;
}

public class UpdateCustomerDto
{
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Username { set; get; } = null!;

    [Required]
    [EmailAddress]
    public string Email { set; get; } = null!;
}