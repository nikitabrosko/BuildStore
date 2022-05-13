using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.GetRolesForSpecifiedUser
{
    public class GetRolesForSpecifiedUserQueryHandler : IRequestHandler<GetRolesForSpecifiedUserQuery, IList<string>>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public GetRolesForSpecifiedUserQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> Handle(GetRolesForSpecifiedUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.UserId);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }

            return await _userManager.GetRolesAsync(entity);
        }
    }
}