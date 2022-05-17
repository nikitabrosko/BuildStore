using System.Collections.Generic;
using Domain.IdentityEntities;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }
        
        public ShoppingCart()
        {
            Products = new HashSet<Product>();
        }
    }
}