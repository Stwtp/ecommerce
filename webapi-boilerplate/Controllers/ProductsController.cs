using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Dtos.Product;
using webapi_boilerplate.Dtos;
using webapi_boilerplate.Models;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<ProductResponseDto>>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "asc")
    {
        var result = _context.Products.Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            result = result.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));
        }
        if (categoryId.HasValue)
        {
            result = result.Where(p => p.CategoryId == categoryId.Value);
        }
        if (minPrice.HasValue)
        {
            result = result.Where(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            result = result.Where(p => p.Price <= maxPrice.Value);
        }
        if (!string.IsNullOrEmpty(sortBy))
        {
            result = sortBy switch
            {
                "name" => sortOrder == "desc" ? result.OrderByDescending(p => p.Name) : result.OrderBy(p => p.Name),
                "price" => sortOrder == "desc" ? result.OrderByDescending(p => p.Price) : result.OrderBy(p => p.Price),
                _ => result.OrderBy(p => p.Name)
            };
        }
        var totalCount = await result.CountAsync();
        result = result.Skip((page - 1) * pageSize).Take(pageSize);
        var response =  new PaginatedResponseDto<ProductResponseDto>
        {
            Items = await result.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                ImageUrl = p.ImageUrl,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                UpdatedBy = p.UpdatedBy
            }).ToListAsync(),
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            UpdatedBy = product.UpdatedBy
        });
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto req)
    {
        var category = await _context.Categories.FindAsync(req.CategoryId);
        if (category == null)
        {
            return BadRequest(new { message = "Category not found" });
        }
        var existProduct = await _context.Products.AnyAsync(p => p.Name == req.Name);
        if(existProduct)
        {
            return BadRequest(new { message = "Product already exists" });
        }
        var product = new Product
        {
            Name = req.Name,
            Description = req.Description,
            Price = req.Price,
            CategoryId = req.CategoryId,
            ImageUrl = req.ImageUrl,
            Stock = req.Stock,
            UpdatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = category.Name,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            UpdatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
        });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<ProductResponseDto>> UpdateProduct(int id, [FromBody] ProductRequestDto req)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        product.Name = req.Name;
        product.Description = req.Description;
        product.Price = req.Price;
        product.CategoryId = req.CategoryId;
        product.ImageUrl = req.ImageUrl;
        product.Stock = req.Stock;
        product.IsActive = req.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        
        return Ok(new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            UpdatedBy = product.UpdatedBy
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
