using ProductsCRUD.Application.DTOs.Input;
using FluentValidation;

namespace ProductsCRUD.Application.DTOs.Validators
{
    internal class ProductInputValidator : AbstractValidator<ProductInput>
    {
        public ProductInputValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name cannot be empty")
                .MaximumLength(100);

            RuleFor(p => p.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage("Description cannot be empty")
                .MaximumLength(500);

            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Price cannot be negative");


        }
    }
}
