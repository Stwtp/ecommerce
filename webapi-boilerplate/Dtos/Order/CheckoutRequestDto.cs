namespace webapi_boilerplate.Dtos.Order;

public class CheckoutRequestDto
{
    public required string ShippingAddress { get; set; }
    public string? Notes { get; set; }
}
