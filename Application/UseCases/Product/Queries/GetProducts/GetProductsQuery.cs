using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Product.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<IEnumerable<Domain.Entities.Product>>
    {
        public int? Count { get; set; }
    }
}
