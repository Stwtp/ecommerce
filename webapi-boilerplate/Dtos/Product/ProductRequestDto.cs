namespace webapi_boilerplate.Dtos.Product;

public class ProductRequestDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public required int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public required int CategoryId { get; set; }
    public bool IsActive { get; set; } = true;
}