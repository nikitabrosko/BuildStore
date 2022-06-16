using Application.Common.Models;

namespace WebUI.Models.Home
{
    public class ModelForShopProductsPartial
    {
        public PaginatedList<Domain.Entities.Product> Products { get; set; }

        public string SearchPattern { get; set; }
    }
}
