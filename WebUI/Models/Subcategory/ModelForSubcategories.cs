using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Subcategory
{
    public class ModelForSubcategories
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<Domain.Entities.Subcategory> Subcategories { get; set; }

        public string SearchPattern { get; set; }
    }
}
