using MediatR;

namespace Application.UseCases.ProductsDictionary.Commands.DeleteProductsDictionary
{
    public class DeleteProductsDictionaryCommand : IRequest
    {
        public int Id { get; set; }
    }
}