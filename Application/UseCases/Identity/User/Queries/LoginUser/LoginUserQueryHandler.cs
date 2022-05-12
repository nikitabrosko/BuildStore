using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, SignInResult>
    {
        private readonly SignInManager<Domain.IdentityEntities.User> _signInManager;
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public LoginUserQueryHandler(SignInManager<Domain.IdentityEntities.User> signInManager, UserManager<Domain.IdentityEntities.User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByNameAsync(request.Name);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.Name);
            }

            return await _signInManager.PasswordSignInAsync(request.Name, request.Password,
                request.RememberMe, false);
        }
    }
}