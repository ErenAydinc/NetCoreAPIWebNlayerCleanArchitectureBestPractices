using App.Application.Contracts.BusServices;
using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using App.Domain.Event;
using AutoMapper;
using FluentValidation;
using System.Net;

namespace App.Application.Features.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IValidator<CreateProductRequest> createProductRequestValidator, IMapper mapper, ICacheService cacheService,IBusService busService) : IProductService
    {
        private const string ProductListCacheKey = "ProductListCacheKey";

        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            var product = await productRepository.GetTopPriceProductAsync(count);

            var productAsDto = mapper.Map<List<ProductDto>>(product);

            #region Manual Mapping
            //var productAsDto = product.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList(); 
            #endregion

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto,
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            //cache aside design pattern

            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if (productListAsCached is not null)
                return ServiceResult<List<ProductDto>>.Success(productListAsCached);

            var products = await productRepository.GetAllAsync();
            #region Manual Mapping
            //var productsAsDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList(); 
            #endregion
            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            await cacheService.AddAsync(ProductListCacheKey, productsAsDto, TimeSpan.FromMinutes(1));

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllList(int pageNumber, int pageSize)
        {
            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);

            #region Manual Mapping
            //var productDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock)).ToList(); 
            #endregion

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetById(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            #region Manuel Mapping
            //var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock); 
            #endregion

            var productAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            //throw new Exception("Kritik seviyede bir hata meydana geldi");

            //2. yol async çalışır
            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);
            if (anyProduct)
                return ServiceResult<CreateProductResponse>.Fail("Product name is exists", HttpStatusCode.BadRequest);

            //3. yol async çalışır
            //var validationResult = await createProductRequestValidator.ValidateAsync(request);
            //if (!validationResult.IsValid)
            //    return ServiceResult<CreateProductResponse>.Fail(validationResult.Errors.Select(x=>x.ErrorMessage).ToList());

            //var product = new Product()
            //{
            //    Name = request.Name,
            //    Price = request.Price,
            //    Stock = request.Stock,
            //};

            var product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product);
            await unitOfWork.SaveChangeAsync();

            await busService.PublishAsync(new ProductAddedEvent(product.Id,product.Name,product.Price));

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), $"api/products/{product.Id}");
        }

        //Update ve delete işlemlerinde zaten biz datayı verdiğimiz için işlem başında geriye bir response dönmeyiz.
        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {

            //Fast fail => ilk önce başarısız durum kontrolü yapılır sonra başarılı durum aksiyonu gerçekleşir.

            //Gard Clauses => If ile kontrolleri sağla valid vb. mümkün olduğunca else kullanma. 

            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != request.Id);
            if (isProductNameExist)
                return ServiceResult.Fail("Product name is exists", HttpStatusCode.BadRequest);

            //product.Name = request.Name;
            //product.Price = request.Price;
            //product.Stock = request.Stock;


            //Burada elimizde zaten bir product nesnesi var (getbyid ile çektiğimiz) bu yüzden ekstra generic vermedik direkt parantez içinde tanımını yaptık.
            var product = mapper.Map<Product>(request);

            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            productRepository.Delete(product!);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest updateProductStockRequest)
        {
            var product = await productRepository.GetByIdAsync(updateProductStockRequest.ProductId);
            if (product is null)
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);

            product.Stock = updateProductStockRequest.Quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
