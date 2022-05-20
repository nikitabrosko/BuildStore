namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public PaymentType Type { get; set; }

        public bool Allowed { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string CreditCardNumber { get; set; }

        public int CardExpMonth { get; set; }

        public int CardExpYear { get; set; }
    }

    public enum PaymentType
    {
        CreditCard
    }
}