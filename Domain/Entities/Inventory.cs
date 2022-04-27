namespace Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public bool IsInStock { get; set; }

        public int UnitsInStock { get; set; }
    }
}