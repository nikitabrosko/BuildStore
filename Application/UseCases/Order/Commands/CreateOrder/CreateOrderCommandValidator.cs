using FluentValidation;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(v => v.Products)
                .NotEmpty().WithMessage("Products cannot be empty!")
                .NotNull().WithMessage("Products cannot be null!");
        }
    }
}