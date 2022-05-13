using FluentValidation;

namespace Application.UseCases.Identity.Role.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(40).WithMessage("Name length cannot be more than 40!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");
        }
    }
}