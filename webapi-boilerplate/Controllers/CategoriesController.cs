using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Models;
using webapi_boilerplate.Dtos.Category;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
    {
        var categories = await context.Categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            UpdatedBy = c.UpdatedBy
        }).ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryRequestDto req)
    {
        var existCategory = await context.Categories.AnyAsync(c => c.Name == req.Name);
        if (existCategory)
        {
            return BadRequest("Category already exists");
        }
        var category = new Category
        {
            Name = req.Name,
            Description = req.Description,
            UpdatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var categoryResponse = new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            UpdatedBy = category.UpdatedBy
        };

        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, categoryResponse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] CategoryRequestDto req)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        category.Name = req.Name;
        category.Description = req.Description;
        category.IsActive = req.IsActive;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        var categoryResponse = new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            UpdatedBy = category.UpdatedBy
        };

        return Ok(categoryResponse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return NoContent();
    }
}