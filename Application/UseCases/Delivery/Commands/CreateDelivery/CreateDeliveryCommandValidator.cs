using FluentValidation;

namespace Application.UseCases.Delivery.Commands.CreateDelivery
{
    public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
    {
        public CreateDeliveryCommandValidator()
        {
        }
    }
}