using System.Collections.Generic;

namespace WebUI.Models.Account
{
    public class ModelForLogin
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }
    }
}