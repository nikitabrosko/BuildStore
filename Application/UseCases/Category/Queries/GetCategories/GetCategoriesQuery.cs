using System.Collections.Generic;
using MediatR;

namespace Application.UseCases.Category.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<IEnumerable<Domain.Entities.Category>>
    {
        
    }
}