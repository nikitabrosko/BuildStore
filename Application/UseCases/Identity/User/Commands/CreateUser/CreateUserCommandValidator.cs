using FluentValidation;

namespace Application.UseCases.Identity.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(40).WithMessage("Name length cannot be more than 40!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");

            RuleFor(v => v.Email)
                .MaximumLength(50).WithMessage("Email length cannot be more than 50!")
                .MinimumLength(5).WithMessage("Email length cannot be less than 5!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .NotNull().WithMessage("Email cannot be null!");

            RuleFor(v => v.Password)
                .MaximumLength(64).WithMessage("Password length cannot be more than 64!")
                .MinimumLength(4).WithMessage("Password length cannot be less than 4!")
                .NotEmpty().WithMessage("Password cannot be empty!")
                .NotNull().WithMessage("Password cannot be null!");
        }
    }
}