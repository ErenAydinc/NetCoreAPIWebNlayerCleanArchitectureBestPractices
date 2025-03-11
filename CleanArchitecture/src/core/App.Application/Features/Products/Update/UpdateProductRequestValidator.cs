using FluentValidation;

namespace App.Application.Features.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Product name is required")
                .NotEmpty().WithMessage("Product name is required")
                .Length(3, 10).WithMessage("Product name range is min3-max10 character");
            //.Must(MustUniqueProductName).WithMessage("Product name is exists");
            //.MustAsync(MustUniqueProductNameAsync).WithMessage("Product name is exists");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price greater than 0");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category value greater than 0");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Product stock range is min1-max100");
        }
    }
}
