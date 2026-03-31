using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using webapi_boilerplate.Data;
using webapi_boilerplate.Dtos.Admin;
using webapi_boilerplate.Dtos;
using webapi_boilerplate.Dtos.Product;
using webapi_boilerplate.Dtos.Order;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/admin/dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardResponseDto>> GetDashboard()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalProducts = await _context.Products.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SumAsync(o => o.TotalPrice);

        var orders = await _context.Orders.ToListAsync();

        return Ok(new DashboardResponseDto
        {
            TotalUsers = totalUsers,
            TotalProducts = totalProducts,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
            ProcessingOrders = orders.Count(o => o.Status == OrderStatus.Processing),
            ShippedOrders = orders.Count(o => o.Status == OrderStatus.Shipped),
            DeliveredOrders = orders.Count(o => o.Status == OrderStatus.Delivered),
            CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled)
        });
    }

    // GET /api/admin/orders
    [HttpGet("orders")]
    public async Task<ActionResult<PaginatedResponseDto<AdminOrderResponseDto>>> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? sortBy = "createdAt",
        [FromQuery] string? sortOrder = "desc")
    {
        var query = _context.Orders.Include(o => o.User).AsQueryable();

        // Filter by status
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
        {
            query = query.Where(o => o.Status == orderStatus);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "totalprice" => sortOrder == "desc" ? query.OrderByDescending(o => o.TotalPrice) : query.OrderBy(o => o.TotalPrice),
            "status" => sortOrder == "desc" ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
            _ => sortOrder == "desc" ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt)
        };

        // Pagination
        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        var orders = await query.Select(o => new AdminOrderResponseDto
        {
            Id = o.Id,
            UserId = o.UserId,
            UserEmail = o.User != null ? o.User.Email : "",
            Status = o.Status.ToString(),
            TotalPrice = o.TotalPrice,
            ShippingAddress = o.ShippingAddress,
            ItemCount = o.OrderItems.Count,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt
        }).ToListAsync();

        return Ok(new PaginatedResponseDto<AdminOrderResponseDto>
        {
            Items = orders,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    // GET /api/admin/orders/{id}
    [HttpGet("orders/{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(new OrderResponseDto
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
        });
    }

    // PUT /api/admin/orders/{id}/status
    [HttpPut("orders/{id}/status")]
    public async Task<ActionResult<AdminOrderResponseDto>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequestDto request)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
        {
            return BadRequest("Invalid status");
        }

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new AdminOrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserEmail = order.User?.Email,
            Status = order.Status.ToString(),
            TotalPrice = order.TotalPrice,
            ShippingAddress = order.ShippingAddress,
            ItemCount = await _context.OrderItems.CountAsync(oi => oi.OrderId == order.Id),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        });
    }

    // GET /api/admin/users
    [HttpGet("users")]
    public async Task<ActionResult<PaginatedResponseDto<AdminUserResponseDto>>> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null)
    {
        var query = _context.Users.AsQueryable();

        // Search by email or name
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(u => u.Email.Contains(search) ||
                                     u.FirstName.Contains(search) ||
                                     u.LastName.Contains(search));
        }

        // Filter by role
        if (!string.IsNullOrEmpty(role) && Enum.TryParse<UserRole>(role, true, out var userRole))
        {
            query = query.Where(u => u.Role == userRole);
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new AdminUserResponseDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Ok(new PaginatedResponseDto<AdminUserResponseDto>
        {
            Items = users,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }

    // GET /api/admin/products
    [HttpGet("products")]
    public async Task<ActionResult<PaginatedResponseDto<ProductResponseDto>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        // Search by name
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalCount = await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(new PaginatedResponseDto<ProductResponseDto>
        {
            Items = products,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
    }
}
