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
    public required string Username { set; get; }
    public required string Password { set; get; }
    public required string Email { set; get; }
}

public class UpdateCustomerDto
{
    public required string Username { set; get; }
    public required string Password { set; get; }
    public required string Email { set; get; }
}