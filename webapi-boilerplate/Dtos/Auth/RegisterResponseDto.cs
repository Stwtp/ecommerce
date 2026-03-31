namespace webapi_boilerplate.Dtos.Auth;

public class RegisterResponseDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; }
}