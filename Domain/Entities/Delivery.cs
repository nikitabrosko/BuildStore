namespace Domain.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        public DeliveryType Type { get; set; }

        public bool Fulfilled { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }

    public enum DeliveryType
    {
        Express,
        Mail
    }
}