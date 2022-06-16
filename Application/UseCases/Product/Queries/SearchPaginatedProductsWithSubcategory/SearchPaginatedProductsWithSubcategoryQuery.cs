using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Product.Queries.SearchPaginatedProductsWithSubcategory
{
    public class SearchPaginatedProductsWithSubcategoryQuery : IRequest<PaginatedList<Domain.Entities.Product>>
    {
        public int SubcategoryId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
