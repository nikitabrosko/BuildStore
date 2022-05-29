using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Product.Queries.GetPaginatedProductsWithSubcategory
{
    public class GetPaginatedProductsWithSubcategoryQuery : IRequest<PaginatedList<Domain.Entities.Product>>
    {
        public int SubcategoryId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}