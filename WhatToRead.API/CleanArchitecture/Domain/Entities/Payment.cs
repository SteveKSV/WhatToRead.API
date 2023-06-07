using Domain.Common;
namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; private set; }
        public string PaymentMethod { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string Status { get; private set; }

        public Payment(string id, decimal amount, string paymentMethod)
        {
            _id = id;
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
