using MediatR;

namespace Application.UseCases.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<Domain.Entities.Category>
    {
        public int Id { get; set; }
    }
}