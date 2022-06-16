using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Subcategory.Queries.GetSubcategoriesWithPagination
{
    public class GetSubcategoriesWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Subcategory>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
