using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Category
{
    public class ModelForCategoryShop
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public IEnumerable<Domain.Entities.Product> Products { get; set; }

        public PaginatedList<Domain.Entities.Product> ProductsPaginated { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }

        public Domain.Entities.CategoryBase Category { get; set; }

        public string SearchPattern { get; set; }
    }
}
