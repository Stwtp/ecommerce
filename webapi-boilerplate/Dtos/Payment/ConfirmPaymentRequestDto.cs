namespace webapi_boilerplate.Dtos.Payment;

public class ConfirmPaymentRequestDto
{
    public required string PaymentIntentId { get; set; }
}
