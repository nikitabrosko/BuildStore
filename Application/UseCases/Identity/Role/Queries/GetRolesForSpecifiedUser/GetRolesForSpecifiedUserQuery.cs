using System.Collections.Generic;
using MediatR;

namespace Application.UseCases.Identity.Role.Queries.GetRolesForSpecifiedUser
{
    public class GetRolesForSpecifiedUserQuery : IRequest<IList<string>>
    {
        public string UserId { get; set; }
    }
}