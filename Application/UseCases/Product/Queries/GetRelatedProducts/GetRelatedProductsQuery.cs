using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Product.Queries.GetRelatedProducts
{
    public class GetRelatedProductsQuery : IRequest<IEnumerable<Domain.Entities.Product>>
    {
        public Domain.Entities.Product Product { get; set; }
    }
}
