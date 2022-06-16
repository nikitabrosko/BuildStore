using Application.Common.Models;

namespace WebUI.Models.Category
{
    public class ModelForShopProductsCategoryPartial
    {
        public PaginatedList<Domain.Entities.Product> Products { get; set; }

        public string SearchPattern { get; set; }

        public int CategoryId { get; set; }
    }
}
