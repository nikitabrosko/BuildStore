using MediatR;

namespace Application.UseCases.ProductsDictionary.Queries.GetProductsDictionary
{
    public class GetProductsDictionaryQuery : IRequest<Domain.Entities.ProductsDictionary>
    {
        public int Id { get; set; }
    }
}
