using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Identity.User.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Domain.IdentityEntities.User>
    {
        private readonly IApplicationIdentityDbContext _identityContext;

        public GetUserQueryHandler(IApplicationIdentityDbContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<Domain.IdentityEntities.User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users
                .Include(u => u.Customer)
                .ThenInclude(c => c.Orders)
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.UserName), cancellationToken);

            return user;
        }
    }
}