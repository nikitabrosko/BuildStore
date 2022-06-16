using System.Collections.Generic;

namespace WebUI.Models.Product
{
    public class ModelForProductDetails
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.Product Product { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }

        public IEnumerable<Domain.Entities.Product> RelatedProducts { get; set; }

        public IEnumerable<Domain.Entities.Product> Products { get; set; }
    }
}
