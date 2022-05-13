using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.IdentityEntities;

namespace Domain.Entities
{
    public class ShoppingCart
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }
        
        public ShoppingCart()
        {
            Products = new HashSet<Product>();
        }
    }
}