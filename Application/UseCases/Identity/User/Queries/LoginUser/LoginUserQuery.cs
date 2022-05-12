using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<SignInResult>
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}