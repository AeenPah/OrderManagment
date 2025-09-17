// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using OrderManagement.Data;
// using OrderManagement.DTOs;
// using OrderManagement.Models;

// namespace OrderManagement.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class OrderItemController : ControllerBase
// {
//     private readonly AppDbContext _dbContext;
//     public OrderItemController(AppDbContext appDbContext) { this._dbContext = appDbContext; }


//     [HttpGet]
//     public async Task<IActionResult> GetOrderItem()
//     {
//         List<OrderItem> orderItem = await _dbContext.OrderItems.ToListAsync();

//         return Ok(orderItem);
//     }

//     [HttpPost]
//     public async Task<IActionResult> PostOrderItem([FromBody] OrderItemRequest body)
//     {

//         OrderItem orderItem = new OrderItem
//         {
//             Name = body.Name,
//             Price = body.Price
//         };

//         _dbContext.OrderItems.Add(orderItem);
//         await _dbContext.SaveChangesAsync();

//         return Ok();
//     }
// }