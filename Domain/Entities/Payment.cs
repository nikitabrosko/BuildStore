namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public PaymentType Type { get; set; }

        public bool Allowed { get; set; }
    }

    public enum PaymentType
    {
        CreditCard
    }
}