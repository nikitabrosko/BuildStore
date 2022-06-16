using Application.Common.Models;
using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.User.Queries.SearchUsersWithRolesWithPagination
{
    public class SearchUsersWithRolesWithPaginationQueryHandler : IRequestHandler<SearchUsersWithRolesWithPaginationQuery, PaginatedList<UserWithRolesDto>>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public SearchUsersWithRolesWithPaginationQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<UserWithRolesDto>> Handle(SearchUsersWithRolesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users
                .Include(u => u.Customer)
                .Where(u => u.UserName.Contains(request.Pattern))
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
