using System.Collections.Generic;

namespace WebUI.Models.Account
{
    public class ModelForMyAccount
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }

        public Domain.IdentityEntities.User User { get; set; }
    }
}
