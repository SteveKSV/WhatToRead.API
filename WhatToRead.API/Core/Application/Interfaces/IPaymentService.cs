using Core.Application.Dtos;

namespace Core.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentDto paymentDto);
        Task<PaymentResult> RefundPaymentAsync(int orderId);
        Task<bool> IsPaymentValidAsync(int orderId);
    }

    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
