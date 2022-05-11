using FluentValidation;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(v => v.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId cannot be less than 1!");

            RuleFor(v => v.SupplierId)
                .GreaterThan(0).WithMessage("SupplierId cannot be less than 1!")
                .NotNull().WithMessage("SupplierId cannot be null!");

            RuleFor(v => v.Name)
                .MaximumLength(100).WithMessage("Name length cannot be more than 100!")
                .MinimumLength(3).WithMessage("Name length cannot be less than 3!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Name length cannot be more than 500!")
                .MinimumLength(50).WithMessage("Name length cannot be less than 50!")
                .NotEmpty().WithMessage("Name cannot be empty!")
                .NotNull().WithMessage("Name cannot be null!");

            RuleFor(v => v.Price)
                .GreaterThan(0).WithMessage("Price cannot be less than or equal to 0!")
                .NotNull().WithMessage("Price cannot be null!");

            RuleFor(v => v.QuantityPerUnit)
                .GreaterThan(0).WithMessage("QuantityPerUnit cannot be less than 1!")
                .NotNull().WithMessage("QuantityPerUnit cannot be null!");

            RuleFor(v => v.Weight)
                .GreaterThan(0).WithMessage("Weight cannot be less than or equal to 0!")
                .NotNull().WithMessage("Weight cannot be null!");

            RuleFor(v => v.Picture)
                .NotNull().WithMessage("Picture cannot be null!");

            RuleFor(v => v.Discount)
                .NotNull().WithMessage("Discount cannot be null!");
        }
    }
}