using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Dtos.Order;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // POST /api/orders/checkout
    [HttpPost("checkout")]
    [Authorize]
    public async Task<ActionResult<OrderResponseDto>> Checkout([FromBody] CheckoutRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        // Get cart items with products
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        if (cartItems.Count == 0)
        {
            return BadRequest("Cart is empty");
        }

        // Validate stock for all items
        foreach (var item in cartItems)
        {
            if (item.Product == null || item.Product.Stock < item.Quantity)
            {
                return BadRequest($"Insufficient stock for product: {item.Product?.Name}");
            }
        }

        // Create order
        var order = new Order
        {
            UserId = userId,
            TotalPrice = cartItems.Sum(c => c.Product!.Price * c.Quantity),
            ShippingAddress = request.ShippingAddress,
            Notes = request.Notes,
            Status = OrderStatus.Pending
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Create order items and deduct stock
        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product!.Price
            };
            _context.OrderItems.Add(orderItem);

            // Deduct stock
            cartItem.Product.Stock -= cartItem.Quantity;
        }

        // Clear cart
        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        // Load order items for response
        await _context.Entry(order).Collection(o => o.OrderItems).LoadAsync();
        foreach (var item in order.OrderItems)
        {
            await _context.Entry(item).Reference(i => i.Product).LoadAsync();
        }

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, MapToOrderResponse(order));
    }

    // GET /api/orders
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<OrderResponseDto>>> GetOrders()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders.Select(MapToOrderResponse).ToList());
    }

    // GET /api/orders/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(MapToOrderResponse(order));
    }

    // PUT /api/orders/{id}/status
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(int id, [FromBody] string status)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
        {
            return BadRequest("Invalid status");
        }

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(MapToOrderResponse(order));
    }

    private static OrderResponseDto MapToOrderResponse(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            ShippingAddress = order.ShippingAddress,
            TotalPrice = order.TotalPrice,
            Items = order.OrderItems.Select(oi => new OrderItemResponseDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name,
                ProductImageUrl = oi.Product?.ImageUrl,
                Quantity = oi.Quantity,
                Price = oi.Price,
                Subtotal = oi.Price * oi.Quantity
            }).ToList(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
