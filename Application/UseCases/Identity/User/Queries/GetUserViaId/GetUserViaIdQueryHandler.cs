using Application.Common.Interfaces;
using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.User.Queries.GetUserViaId
{
    public class GetUserViaIdQueryHandler : IRequestHandler<GetUserViaIdQuery, UserWithRolesDto>
    {
        private readonly IApplicationIdentityDbContext _identityContext;
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public GetUserViaIdQueryHandler(IApplicationIdentityDbContext identityContext,
            UserManager<Domain.IdentityEntities.User> userManager)
        {
            _identityContext = identityContext;
            _userManager = userManager;
        }

        public async Task<UserWithRolesDto> Handle(GetUserViaIdQuery request, CancellationToken cancellationToken)
        {
            var user = new UserWithRolesDto
            {
                User = await _identityContext.Users
                .Include(u => u.Customer)
                .ThenInclude(c => c.Orders)
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken)
            };

            user.Roles = await _userManager.GetRolesAsync(user.User);

            return user;
        }
    }
}
