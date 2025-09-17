using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.DTOs;
using OrderManagement.Models;

namespace OrderManagement;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public OrdersController(AppDbContext appDbContext) { this._dbContext = appDbContext; }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .Select(o => new OrderDTO(
                o.OrderId,
                o.CustomerId,
                o.TotalAmount,
                o.OrderItems.Select(oi => new OrderItemDTO(oi.ProductName, oi.Quantity, oi.Price)).ToList()
            ))
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdOrders(int id)
    {
        var order = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderId == id)
            .Select(o => new OrderDTO(
                o.OrderId,
                o.CustomerId,
                o.TotalAmount,
                o.OrderItems.Select(oi => new OrderItemDTO(oi.ProductName, oi.Quantity, oi.Price)).ToList()
            ))
            .ToListAsync();

        if (order.Count() == 0)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> PostOrder([FromBody] CreateOrderDTO body)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existedCustomer = await _dbContext.Customer
                   .FirstOrDefaultAsync(c => c.CustomerId == body.CustomerId);
        if (existedCustomer is null)
            return NotFound($"Customer with ID {body.CustomerId} not found.");

        Order order = new Order
        {
            CustomerId = body.CustomerId,
            TotalAmount = 0,
            OrderItems = body.OrderItems.Select(oi => new OrderItem
            {
                ProductName = oi.ProductName,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        };

        // calculate total amount price
        order.TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        OrderDTO orderDTO = new OrderDTO
        (
            order.OrderId,
            order.CustomerId,
            order.TotalAmount,
            order.OrderItems
                .Select(oi => new OrderItemDTO(oi.ProductName, oi.Quantity, oi.Price))
                .ToList()
        );

        return Ok(orderDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutOrder(int id, [FromBody] UpdateOrderDTO body)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existedCustomer = await _dbContext.Customer
              .FirstOrDefaultAsync(c => c.CustomerId == body.CustomerId);
        if (existedCustomer is null)
            return NotFound($"Customer with ID {body.CustomerId} not found.");

        var existingOrder = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (existingOrder is null)
            return NotFound($"Order with ID {id} not found.");

        existingOrder.CustomerId = body.CustomerId;
        existingOrder.Status = body.Status;

        // remove old items
        _dbContext.OrderItems.RemoveRange(existingOrder.OrderItems);

        var newItems = body.OrderItems.Select(oi => new OrderItem
        {
            OrderId = existingOrder.OrderId,
            ProductName = oi.ProductName,
            Quantity = oi.Quantity,
            Price = oi.Price
        }).ToList();

        // calculate total amount price
        existingOrder.TotalAmount = newItems.Sum(oi => oi.Price * oi.Quantity);

        _dbContext.OrderItems.AddRange(newItems);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        if (order is null)
            return NotFound($"Order with ID {id} not found.");

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

}