using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; private set; }
        public string PaymentMethod { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string Status { get; private set; }

        public Payment(int id, decimal amount, string paymentMethod)
        {
            Id = id;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentDate = DateTime.UtcNow;
            Status = "Pending";
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
        }

        public void MarkAsFailed()
        {
            Status = "Failed";
        }
    }

}
