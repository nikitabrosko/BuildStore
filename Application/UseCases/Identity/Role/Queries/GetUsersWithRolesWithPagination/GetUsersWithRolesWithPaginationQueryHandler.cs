using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.GetUsersWithRolesWithPagination
{
    public class GetUsersWithRolesWithPaginationQueryHandler : IRequestHandler<GetUsersWithRolesWithPaginationQuery, PaginatedList<UserWithRolesDto>>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public GetUsersWithRolesWithPaginationQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserWithRolesDto>> Handle(GetUsersWithRolesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users
                .Select(user => new UserWithRolesDto
                {
                    Id = user.Id,
                    Name = user.UserName
                }).ToList();

            foreach (var user in query)
            {
                var entity = await _userManager.FindByIdAsync(user.Id);

                user.Roles = await _userManager.GetRolesAsync(entity);
            }

            return PaginatedList<UserWithRolesDto>.Create(query.AsQueryable(), request.PageNumber, request.PageSize);
        }
    }
}