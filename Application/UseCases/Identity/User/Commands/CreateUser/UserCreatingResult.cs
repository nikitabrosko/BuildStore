using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Commands.CreateUser
{
    public class UserCreatingResult
    {
        public IdentityResult Result { get; set; }

        public Domain.IdentityEntities.User User { get; set; }
    }
}