using MediatR;

namespace Application.UseCases.Identity.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}