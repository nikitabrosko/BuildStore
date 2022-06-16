using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using MediatR;

namespace Application.UseCases.Identity.User.Queries.GetUserViaId
{
    public class GetUserViaIdQuery : IRequest<UserWithRolesDto>
    {
        public string Id { get; set; }
    }
}
