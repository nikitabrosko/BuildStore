using FluentValidation;

namespace Application.UseCases.Supplier.Commands.CreateSupplier
{
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(v => v.CompanyName)
                .MaximumLength(100).WithMessage("CompanyName length cannot be more than 100!")
                .MinimumLength(3).WithMessage("CompanyName length cannot be less than 3!")
                .NotEmpty().WithMessage("CompanyName cannot be empty!")
                .NotNull().WithMessage("CompanyName cannot be null!");

            RuleFor(v => v.Address)
                .MaximumLength(100).WithMessage("Address length cannot be more than 100!")
                .MinimumLength(5).WithMessage("Address length cannot be less than 5!")
                .NotEmpty().WithMessage("Address cannot be empty!")
                .NotNull().WithMessage("Address cannot be null!");

            RuleFor(v => v.City)
                .MaximumLength(50).WithMessage("City length cannot be more than 50!")
                .MinimumLength(1).WithMessage("City length cannot be less than 1!")
                .NotEmpty().WithMessage("City cannot be empty!")
                .NotNull().WithMessage("City cannot be null!");

            RuleFor(v => v.Country)
                .MaximumLength(50).WithMessage("Country length cannot be more than 50!")
                .MinimumLength(1).WithMessage("Country length cannot be less than 1!")
                .NotEmpty().WithMessage("Country cannot be empty!")
                .NotNull().WithMessage("Country cannot be null!");

            RuleFor(v => v.Email)
                .MaximumLength(50).WithMessage("Email length cannot be more than 50!")
                .MinimumLength(5).WithMessage("Email length cannot be less than 5!")
                .NotEmpty().WithMessage("Email cannot be empty!")
                .NotNull().WithMessage("Email cannot be null!");

            RuleFor(v => v.PhoneNumber)
                .MaximumLength(50).WithMessage("PhoneNumber length cannot be more than 50!")
                .MinimumLength(1).WithMessage("PhoneNumber length cannot be less than 1!")
                .NotEmpty().WithMessage("PhoneNumber cannot be empty!")
                .NotNull().WithMessage("PhoneNumber cannot be null!");
        }
    }
}