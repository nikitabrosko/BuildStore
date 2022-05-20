using System;
using FluentValidation;

namespace Application.UseCases.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(100).WithMessage("FirstName length cannot be more than 50!")
                .MinimumLength(3).WithMessage("FirstName length cannot be less than 2!")
                .NotEmpty().WithMessage("FirstName cannot be empty!")
                .NotNull().WithMessage("FirstName cannot be null!");

            RuleFor(v => v.LastName)
                .MaximumLength(100).WithMessage("LastName length cannot be more than 50!")
                .MinimumLength(3).WithMessage("LastName length cannot be less than 2!")
                .NotEmpty().WithMessage("LastName cannot be empty!")
                .NotNull().WithMessage("LastName cannot be null!");

            RuleFor(v => v.Address)
                .MaximumLength(100).WithMessage("Address length cannot be more than 100!")
                .MinimumLength(10).WithMessage("Address length cannot be less than 10!")
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

            RuleFor(v => v.Phone)
                .MaximumLength(50).WithMessage("PhoneNumber length cannot be more than 50!")
                .MinimumLength(1).WithMessage("PhoneNumber length cannot be less than 1!")
                .NotEmpty().WithMessage("PhoneNumber cannot be empty!")
                .NotNull().WithMessage("PhoneNumber cannot be null!");
        }
    }
}