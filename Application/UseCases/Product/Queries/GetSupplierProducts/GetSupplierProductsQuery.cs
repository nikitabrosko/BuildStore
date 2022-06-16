using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Product.Queries.GetSupplierProducts
{
    public class GetSupplierProductsQuery : IRequest<IEnumerable<Domain.Entities.Product>>
    {
        public Domain.Entities.Supplier Supplier { get; set; }
    }
}
