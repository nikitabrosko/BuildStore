using MediatR;

namespace Application.UseCases.Subcategory.Queries.GetSubcategory
{
    public class GetSubcategoryQuery : IRequest<Domain.Entities.Subcategory>
    {
        public int Id { get; set; }
    }
}