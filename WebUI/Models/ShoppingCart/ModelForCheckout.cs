using System.Collections.Generic;

namespace WebUI.Models.ShoppingCart
{
    public class ModelForCheckout
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }

        public ModelForCheckoutForm ModelForForm { get; set; }
    }
}
