using MediatR;

namespace Application.UseCases.Identity.User.Queries.GetUser
{
    public class GetUserQuery : IRequest<Domain.IdentityEntities.User>
    {
        public string UserName { get; set; }
    }
}