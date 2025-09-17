using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.DTOs;
using OrderManagement.Models;
using OrderManagement.Utils;

namespace OrderManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public CustomersController(AppDbContext appDbContext) { this._dbContext = appDbContext; }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        List<Customer> customers = await _dbContext.Customer.ToListAsync();

        return Ok(customers);
    }

    [HttpPost]
    public async Task<IActionResult> PostCustomer([FromBody] PostCustomer body)
    {
        var (hashedPassword, salt) = PasswordHasher.Hash(body.Password);
        Customer customer = new Customer
        {
            Username = body.Username,
            Email = body.Email,
            PasswordHash = hashedPassword,
            Salt = salt,
        };

        _dbContext.Customer.Add(customer);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    // [HttpGet]
    // public async Task<IActionResult> GetCustomers(int page = 1, int pageSize = 10)
    // {
    //     var customers = await _dbContext.Customer
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToListAsync();

    //     return Ok(customers);
    // }
}