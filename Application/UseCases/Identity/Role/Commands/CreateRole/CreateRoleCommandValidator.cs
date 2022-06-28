using FluentValidation;

namespace Application.UseCases.Identity.Role.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(256).WithMessage("Name length cannot be more than 256!")
                .MinimumLength(1).WithMessage("Name length cannot be less than 1!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");
        }
    }
}