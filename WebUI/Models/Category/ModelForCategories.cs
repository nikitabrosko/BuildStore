using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Category
{
    public class ModelForCategories
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<Domain.Entities.Category> Categories { get; set; }

        public string SearchPattern { get; set; }
    }
}
