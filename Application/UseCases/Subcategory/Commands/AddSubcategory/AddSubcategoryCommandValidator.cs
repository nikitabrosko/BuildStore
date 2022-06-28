﻿using FluentValidation;

namespace Application.UseCases.Subcategory.Commands.AddSubcategory
{
    public class AddSubcategoryCommandValidator : AbstractValidator<AddSubcategoryCommand>
    {
        public AddSubcategoryCommandValidator()
        {
            RuleFor(v => v.SubcategoryId)
                .GreaterThan(0).WithMessage("SubcategoryId cannot be less than 1!");

            RuleFor(v => v.Name)
                .MaximumLength(50).WithMessage("Name length cannot be more than 50!")
                .MinimumLength(2).WithMessage("Name length cannot be less than 2!")
                .NotEmpty().WithMessage("Name cannot be empty")
                .NotNull().WithMessage("Name cannot be null");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Description length cannot be more than 500!")
                .NotNull().WithMessage("Description cannot be null");
        }
    }
}