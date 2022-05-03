using System.Collections.Generic;
using MediatR;

namespace Application.UseCases.Subcategory.Queries.GetSubcategories
{
    public class GetSubcategoriesQuery : IRequest<IEnumerable<Domain.Entities.Subcategory>>
    {
        
    }
}