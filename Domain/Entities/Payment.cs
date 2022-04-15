namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public bool Allowed { get; set; }
    }
}