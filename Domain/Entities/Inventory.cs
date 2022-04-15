namespace Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }

        public Product ProductId { get; set; }

        public float[] AvailableSizes { get; set; }

        public string[] AvailableColors { get; set; }

        public bool IsInStock { get; set; }

        public int UnitsIsInStock { get; set; }
    }
}