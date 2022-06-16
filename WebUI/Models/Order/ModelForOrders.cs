using Application.Common.Models;
using System.Collections.Generic;

namespace WebUI.Models.Order
{
    public class ModelForOrders
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<Domain.Entities.Order> Orders { get; set; }
    }
}
