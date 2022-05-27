using FluentValidation;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(v => v.ProductsDictionary)
                .NotEmpty().WithMessage("ProductsDictionary cannot be empty!")
                .NotNull().WithMessage("ProductsDictionary cannot be null!");
        }
    }
}