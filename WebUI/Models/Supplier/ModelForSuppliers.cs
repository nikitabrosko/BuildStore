using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Supplier
{
    public class ModelForSuppliers
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<Domain.Entities.Supplier> Suppliers { get; set; }

        public string SearchPattern { get; set; }
    }
}
