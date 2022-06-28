using FluentValidation;

namespace Application.UseCases.Identity.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(40).WithMessage("Name length cannot be more than 40!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!");

            RuleFor(v => v.Email)
                .MaximumLength(50).WithMessage("Email length cannot be more than 50!")
                .MinimumLength(5).WithMessage("Email length cannot be less than 5!");

            RuleFor(v => v.Password)
                .MaximumLength(64).WithMessage("Password length cannot be more than 64!")
                .MinimumLength(4).WithMessage("Password length cannot be less than 4!");
        }
    }
}