using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;

namespace OrderManagement;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public CustomerController(AppDbContext appDbContext) { this._dbContext = appDbContext; }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {

        List<Customer> customers = await _dbContext.Customer.ToListAsync();

        return Ok(customers);
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