using MediatR;

namespace Application.UseCases.Identity.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public string Id { get; set; }
    }
}