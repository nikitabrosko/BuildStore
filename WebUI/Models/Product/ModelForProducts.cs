using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Product
{
    public class ModelForProducts
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<Domain.Entities.Product> Products { get; set; }

        public IEnumerable<Domain.Entities.Supplier> Suppliers { get; set; }

        public string SearchPattern { get; set; }
    }
}
