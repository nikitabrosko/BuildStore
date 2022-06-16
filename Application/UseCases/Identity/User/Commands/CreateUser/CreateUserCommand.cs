using MediatR;

namespace Application.UseCases.Identity.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserCreatingResult>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}