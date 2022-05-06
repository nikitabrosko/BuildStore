using MediatR;

namespace Application.UseCases.Supplier.Queries.GetSupplier
{
    public class GetSupplierQuery : IRequest<Domain.Entities.Supplier>
    {
        public int Id { get; set; }
    }
}