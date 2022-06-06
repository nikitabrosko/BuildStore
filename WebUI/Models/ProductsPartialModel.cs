using Application.Common.Models;
using Domain.Entities;

namespace WebUI.Models
{
    public class ProductsPartialModel
    {
        public PaginatedList<Product> Products { get; set; }

        public Category Category { get; set; }
    }
}