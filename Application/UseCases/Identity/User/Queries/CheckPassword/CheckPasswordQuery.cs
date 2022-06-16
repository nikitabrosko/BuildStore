using MediatR;

namespace Application.UseCases.Identity.User.Queries.CheckPassword
{
    public class CheckPasswordQuery : IRequest<bool>
    {
        public Domain.IdentityEntities.User User { get; set; }

        public string Password { get; set; }
    }
}
