using FluentValidation;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.InitialQuantity)
            .GreaterThanOrEqualTo(0);
    }
}