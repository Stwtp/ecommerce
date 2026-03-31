using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Dtos.Cart;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;
    public CartController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<CartItemResponseDto>>> GetCart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == int.Parse(userId ?? "-1"))
            .Include(c => c.Product)
            .Select(c => new CartItemResponseDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductName = c.Product != null ? c.Product.Name : "",
                Quantity = c.Quantity,
                Price = c.Product != null ? c.Product.Price : 0.00m,
                Subtotal = c.Product != null ? c.Product.Price * c.Quantity : 0.00m,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return Ok(cartItems);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CartItemResponseDto>> AddToCart([FromBody] CartItemRequestDto request)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItem = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == int.Parse(userId ?? "-1") && c.ProductId == request.ProductId)
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return BadRequest("Product not found");
        }
        var cartItemQuantity = await _context.CartItems
            .Where(c => c.ProductId == request.ProductId)
            .SumAsync(c => c.Quantity);
        if(product.Stock < request.Quantity || product.Stock <= cartItemQuantity)
        {
            return BadRequest("Product is out of stock");
        }
        if (cartItem != null)
        {
            cartItem.Quantity += request.Quantity;
            await _context.SaveChangesAsync();

            var cartItemResponseDto = new CartItemResponseDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product != null ? cartItem.Product.Name : "",
                Quantity = cartItem.Quantity,
                Price = cartItem.Product != null ? cartItem.Product.Price : 0.00m,
                Subtotal = cartItem.Product != null ? cartItem.Product.Price * cartItem.Quantity : 0.00m,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt
            };
            return Ok(cartItemResponseDto);
        }
        else
        {
            var newCartItem = new CartItem
            {
                UserId = int.Parse(userId ?? "-1"),
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            _context.CartItems.Add(newCartItem);
            await _context.SaveChangesAsync();
            await _context.Entry(newCartItem).Reference(c => c.Product).LoadAsync();
            var cartItemResponseDto = new CartItemResponseDto
            {
                Id = newCartItem.Id,
                ProductId = newCartItem.ProductId,
                ProductName = newCartItem.Product != null ? newCartItem.Product.Name : "",
                Quantity = newCartItem.Quantity,
                Price = newCartItem.Product != null ? newCartItem.Product.Price : 0.00m,
                Subtotal = newCartItem.Product != null ? newCartItem.Product.Price * newCartItem.Quantity : 0.00m,
                CreatedAt = newCartItem.CreatedAt,
                UpdatedAt = newCartItem.UpdatedAt
            };
            return Ok(cartItemResponseDto);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CartItemResponseDto>> UpdateCartItem(int id, [FromBody] UpdateCartItemRequestDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItem = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.Id == id && c.UserId == int.Parse(userId ?? "-1"))
            .FirstOrDefaultAsync();

        if (cartItem == null)
        {
            return NotFound();
        }

        // Check stock
        if (cartItem.Product == null || cartItem.Product.Stock < request.Quantity)
        {
            return BadRequest("Product is out of stock");
        }

        cartItem.Quantity = request.Quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new CartItemResponseDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            ProductName = cartItem.Product.Name,
            Quantity = cartItem.Quantity,
            Price = cartItem.Product.Price,
            Subtotal = cartItem.Product.Price * cartItem.Quantity,
            CreatedAt = cartItem.CreatedAt,
            UpdatedAt = cartItem.UpdatedAt
        });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItem = await _context.CartItems
            .Where(c => c.Id == id && c.UserId == int.Parse(userId ?? "-1"))
            .FirstOrDefaultAsync();

        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteAllCartItems()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == int.Parse(userId ?? "-1"))
            .ToListAsync();

        if (cartItems != null)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}