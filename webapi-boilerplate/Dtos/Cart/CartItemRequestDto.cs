namespace webapi_boilerplate.Dtos.Cart;

public class CartItemRequestDto
{
    public required int ProductId { get; set; }
    public required int Quantity { get; set; }
}