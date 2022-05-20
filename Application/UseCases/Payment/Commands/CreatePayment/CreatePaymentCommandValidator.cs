using System;
using FluentValidation;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(v => v.CreditCardNumber)
                .Length(16).WithMessage("CreditCardNumber length is not equal 16!")
                .NotEmpty().WithMessage("CreditCardNumber cannot be empty!")
                .NotNull().WithMessage("CreditCardNumber cannot be null!");

            RuleFor(v => v.CardExpMonth)
                .LessThanOrEqualTo(12).WithMessage("CardExpMonth cannot be greater than 12!")
                .GreaterThanOrEqualTo(1).WithMessage("CardExpMonth cannot be less than 1!")
                .NotEmpty().WithMessage("CardExpMonth cannot be empty!")
                .NotNull().WithMessage("CardExpMonth cannot be null!");

            RuleFor(v => v.CardExpYear)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage($"CardExpYear cannot be less than {DateTime.Now.Year}!")
                .NotEmpty().WithMessage("CardExpYear cannot be empty!")
                .NotNull().WithMessage("CardExpYear cannot be null!");
        }
    }
}