using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.User.Queries.CheckPassword
{
    public class CheckPasswordQueryHandler : IRequestHandler<CheckPasswordQuery, bool>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public CheckPasswordQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(CheckPasswordQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.CheckPasswordAsync(request.User, request.Password);
        }
    }
}
