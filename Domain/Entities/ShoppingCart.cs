using System.Collections.Generic;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public ShoppingCart()
        {
            Products = new HashSet<Product>();
        }
    }
}