using App.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

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


        //private async Task<bool> MustUniqueProductNameAsync(string name,CancellationToken cancellationToken)
        //{
        //    return !await _productRepository.Where(x => x.Name == name).AnyAsync();
        //}

        //1. yol ama sync çalışır

        //private bool MustUniqueProductName(string name)
        //{
        //    return !_productRepository.Where(x => x.Name == name).Any();
        //}
    }
}
