namespace Domain.Entities
{
    public class ProductsDictionary
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public int Count { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public Order Order { get; set; }
    }
}