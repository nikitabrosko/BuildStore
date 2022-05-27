using System.Collections.Generic;
using Domain.IdentityEntities;

namespace Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ICollection<ProductsDictionary> ProductsDictionary { get; set; }
        
        public ShoppingCart()
        {
            ProductsDictionary = new HashSet<ProductsDictionary>();
        }
    }
}