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
    public async Task<IActionResult> GetCustomers(int page = 1, int pageSize = 10)
    {
        var totalCount = await _dbContext.Customer.CountAsync();

        List<CustomerDTO> customersDTO = await _dbContext.Customer
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CustomerDTO
            (
                c.CustomerId,
                c.Username,
                c.Email,
                c.WalletBalance,
                c.CreatedAt
            )).ToListAsync();

        var response = new PaginatedResponse<CustomerDTO>(customersDTO, totalCount, page, pageSize);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdCustomer(int id)
    {
        var customerDTO = await _dbContext.Customer
            .Where(c => c.CustomerId == id)
            .Select(c => new CustomerDTO
                (
                    c.CustomerId,
                    c.Username,
                    c.Email,
                    c.WalletBalance,
                    c.CreatedAt
                ))
            .FirstOrDefaultAsync();

        if (customerDTO is null)
            return NotFound($"Customer with ID {id} not found.");

        return Ok(customerDTO);
    }

    [HttpPost]
    public async Task<IActionResult> PostCustomer([FromBody] CreateCustomerDto body)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Avoid create duplicated customer
        var existedCustomer = await _dbContext.Customer
            .FirstOrDefaultAsync(c => c.Username == body.Username || c.Email == body.Email);
        if (existedCustomer is not null)
        {
            if (existedCustomer.Username == body.Username)
                return Conflict("Customer with the same username already exists.");

            if (existedCustomer.Email == body.Email)
                return Conflict("Customer with the same email already exists.");
        }

        // Hash Password
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

        var customerDTO = new CustomerDTO
        (
            customer.CustomerId,
            customer.Username,
            customer.Email,
            customer.WalletBalance,
            customer.CreatedAt
        );

        return Created($"/customers/{customer.CustomerId}", customerDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, [FromBody] UpdateCustomerDto body)
    {
        var customer = await _dbContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == id);
        if (customer is null)
            return NotFound($"Customer with ID {id} not found.");

        // Avoid create duplicated customer
        var existedCustomer = await _dbContext.Customer
            .FirstOrDefaultAsync(c => (c.Username == body.Username || c.Email == body.Email) && c.CustomerId != id);
        if (existedCustomer is not null)
        {
            if (existedCustomer.Username == body.Username)
                return Conflict("Customer with the same username already exists.");

            if (existedCustomer.Email == body.Email)
                return Conflict("Customer with the same email already exists.");
        }

        customer.Username = body.Username;
        customer.Email = body.Email;

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _dbContext.Customer.FirstOrDefaultAsync(c => c.CustomerId == id);
        if (customer is null)
            return NotFound($"Customer with ID {id} not found.");

        var hasOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.CustomerId == id);
        if (hasOrder is not null)
            return BadRequest("Customer has order and cannot be deleted.");

        _dbContext.Customer.Remove(customer);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}