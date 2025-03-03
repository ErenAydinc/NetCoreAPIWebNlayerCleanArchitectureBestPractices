using App.Repositories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var product = await productRepository.GetTopPriceProductAsync(count);

            var productAsDto = product.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();


            return new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto,
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();
            var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllList(int pageNumber,int pageSize)
        {
            var products = await productRepository.GetAll().Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
            var productDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList();
            return ServiceResult<List<ProductDto>>.Success(productDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetById(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                ServiceResult<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
            };

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
        }

        //Update ve delete işlemlerinde zaten biz datayı verdiğimiz için işlem başında geriye bir response dönmeyiz.
        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {

            //Fast fail => ilk önce başarısız durum kontrolü yapılır sonra başarılı durum aksiyonu gerçekleşir.

            //Gard Clauses => If ile kontrolleri sağla valid vb. mümkün olduğunca else kullanma. 

            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);

            productRepository.Delete(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
