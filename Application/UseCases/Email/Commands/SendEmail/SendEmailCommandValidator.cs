using FluentValidation;

namespace Application.UseCases.Email.Commands.SendEmail
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(v => v.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress cannot be empty!")
                .NotNull().WithMessage("EmailAddress cannot be null!");
        }
    }
}