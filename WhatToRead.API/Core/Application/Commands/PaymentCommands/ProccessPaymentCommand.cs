using Core.Application.Dtos;

namespace Core.Application.Commands.PaymentCommands
{
    public class ProcessPaymentCommand
    {
        public int OrderId { get; set; }
        public PaymentDto Payment { get; set; }
        // Additional properties for payment processing
    }
}
