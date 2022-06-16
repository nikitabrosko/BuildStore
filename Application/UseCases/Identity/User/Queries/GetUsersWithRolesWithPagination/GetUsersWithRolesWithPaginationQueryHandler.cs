using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination
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
                .Include(u => u.Customer)
                .Select(user => new UserWithRolesDto
                {
                    User = user
                }).ToList();

            foreach (var user in query)
            {
                var entity = await _userManager.FindByIdAsync(user.User.Id);

                user.Roles = await _userManager.GetRolesAsync(entity);
            }

            return PaginatedList<UserWithRolesDto>.Create(query.AsQueryable(), request.PageNumber, request.PageSize);
        }
    }
}