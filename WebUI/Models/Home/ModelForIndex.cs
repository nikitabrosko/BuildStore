using System.Collections.Generic;

namespace WebUI.Models.Home
{
    public class ModelForIndex
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public IEnumerable<Domain.Entities.Product> Products { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }
    }
}