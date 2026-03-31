export interface PaymentResponse {
  clientSecret: string;
  paymentIntentId: string;
  amount: number;
  currency: string;
}
