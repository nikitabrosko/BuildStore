using FluentValidation;

namespace Application.UseCases.Category.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50).WithMessage("Name length cannot be more than 50!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!")
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be null");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Description length cannot be more than 500!")
                .NotNull().WithMessage("Description cannot be null");

            RuleFor(v => v.Picture)
                .NotNull().WithMessage("Picture cannot be null!");
        }
    }
}