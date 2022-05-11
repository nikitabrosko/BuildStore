using FluentValidation;

namespace Application.UseCases.Subcategory.Commands.UpdateSubcategory
{
    public class UpdateSubcategoryCommandValidator : AbstractValidator<UpdateSubcategoryCommand>
    {
        public UpdateSubcategoryCommandValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0).WithMessage("Id cannot be less than 1!");

            RuleFor(v => v.Name)
                .MaximumLength(100).WithMessage("Name length cannot be more than 100!")
                .MinimumLength(3).WithMessage("Name length cannot be less than 3!")
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be null");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Description length cannot be more than 500!")
                .NotNull().WithMessage("Description cannot be null");
        }
    }
}