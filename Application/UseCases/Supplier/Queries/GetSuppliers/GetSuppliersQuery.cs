using System.Collections.Generic;
using MediatR;

namespace Application.UseCases.Supplier.Queries.GetSuppliers
{
    public class GetSuppliersQuery : IRequest<IEnumerable<Domain.Entities.Supplier>>
    {
        
    }
}