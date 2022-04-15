namespace Domain.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public bool Fulfilled { get; set; }
    }
}