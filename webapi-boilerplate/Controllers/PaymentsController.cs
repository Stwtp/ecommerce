using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Dtos.Payment;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // POST /api/payments/create-intent
    [HttpPost("create-intent")]
    [Authorize]
    public async Task<ActionResult<CreatePaymentResponseDto>> CreatePaymentIntent([FromBody] CreatePaymentRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        // Find the order
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

        if (order == null)
        {
            return NotFound("Order not found");
        }

        // Check if payment already exists
        if (!string.IsNullOrEmpty(order.PaymentIntentId))
        {
            // Return existing payment intent
            return Ok(new CreatePaymentResponseDto
            {
                ClientSecret = $"mock_secret_{order.PaymentIntentId}",
                PaymentIntentId = order.PaymentIntentId,
                Amount = order.TotalPrice,
                Currency = "usd"
            });
        }

        // Create mock payment intent
        var mockPaymentId = $"mock_{Guid.NewGuid()}";

        // Save PaymentIntentId to order
        order.PaymentIntentId = mockPaymentId;
        await _context.SaveChangesAsync();

        return Ok(new CreatePaymentResponseDto
        {
            ClientSecret = $"mock_secret_{mockPaymentId}",
            PaymentIntentId = mockPaymentId,
            Amount = order.TotalPrice,
            Currency = "usd"
        });
    }

    // POST /api/payments/confirm
    [HttpPost("confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        // Find the order
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.PaymentIntentId == request.PaymentIntentId && o.UserId == userId);

        if (order == null)
        {
            return NotFound("Order not found");
        }

        // Simulate successful payment
        order.Status = OrderStatus.Processing;
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Payment successful", orderId = order.Id });
    }
}
