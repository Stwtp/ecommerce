using System.ComponentModel.DataAnnotations;

namespace webapi_boilerplate.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = 0.00M;
    public int Stock { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
}