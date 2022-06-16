using System.Collections.Generic;

namespace WebUI.Models.Home
{
    public class ModelForAbout
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }
    }
}
