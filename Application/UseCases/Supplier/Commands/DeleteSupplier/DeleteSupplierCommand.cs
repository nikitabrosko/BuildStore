using MediatR;

namespace Application.UseCases.Supplier.Commands.DeleteSupplier
{
    public class DeleteSupplierCommand : IRequest
    {
        public int Id { get; set; }

        public bool ProductsDeletion { get; set; }
    }
}