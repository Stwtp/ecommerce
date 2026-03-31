namespace webapi_boilerplate.Dtos.Category;

public class CategoryRequestDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}