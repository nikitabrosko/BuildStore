using FluentValidation;

namespace Application.UseCases.Identity.User.Queries.LoginUser
{
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(40).WithMessage("Name length cannot be more than 40!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");

            RuleFor(v => v.Password)
                .MaximumLength(30).WithMessage("Password length cannot be more than 30!")
                .MinimumLength(4).WithMessage("Password length cannot be less than 4!")
                .NotEmpty().WithMessage("Password cannot be empty!")
                .NotNull().WithMessage("Password cannot be null!");
        }
    }
}