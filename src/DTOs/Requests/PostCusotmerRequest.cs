namespace OrderManagement.DTOs;

public class PostCustomer
{
    public required string Username { set; get; }
    public required string Password { set; get; }
    public required string Email { set; get; }
}