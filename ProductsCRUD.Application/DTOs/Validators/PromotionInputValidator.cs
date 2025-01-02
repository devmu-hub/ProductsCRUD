using ProductsCRUD.Application.DTOs.Input;
using FluentValidation;

namespace ProductsCRUD.Application.DTOs.Validators
{
    public class PromotionInputValidator : AbstractValidator<PromotionInput>
    {
        public PromotionInputValidator()
        {
            RuleFor(p => p.PromotionTypeId)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.DiscountPercentage)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

        }
    }
}
