using MediatR;

namespace Application.UseCases.Identity.User.Queries.CheckUserNameForExists
{
    public class CheckUserNameForExistsQuery : IRequest<bool>
    {
        public string UserName { get; set; }
    }
}
