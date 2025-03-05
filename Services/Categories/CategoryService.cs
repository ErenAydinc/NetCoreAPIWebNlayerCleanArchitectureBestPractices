using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.Products;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository,IMapper mapper,IUnitOfWork unitOfWork) : ICategoryService
    {
        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int id)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(id);
            if(category is null)
                return ServiceResult<CategoryWithProductsDto>.Fail("Category not found", HttpStatusCode.NotFound);

            var dto = mapper.Map<CategoryWithProductsDto>(category);
            return ServiceResult<CategoryWithProductsDto>.Success(dto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
        {
            var category =await categoryRepository.GetCategoryWithProducts().ToListAsync();
            if (category is null)
                return ServiceResult<List<CategoryWithProductsDto>>.Fail("Category not found", HttpStatusCode.NotFound);

            var dto = mapper.Map<List<CategoryWithProductsDto>>(category);
            return ServiceResult<List<CategoryWithProductsDto>>.Success(dto);
        }

        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();
            var dto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(dto);
        }
        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
                return ServiceResult<CategoryDto>.Fail("Category not found", HttpStatusCode.NotFound);
            var dto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.Success(dto);
        }
        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();
            if(anyCategory)
                return ServiceResult<int>.Fail("Category already exists",HttpStatusCode.NotFound);

            var newCategory = mapper.Map<Category>(request);
            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }
        public async Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request)
        {
            var category = await categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return ServiceResult.Fail("Category not found", HttpStatusCode.NotFound);

            var isCategoryNameExist= await categoryRepository.Where(x => x.Name == request.Name&&x.Id!=category.Id).AnyAsync();
            if(isCategoryNameExist)
                return ServiceResult.Fail("Category already exists", HttpStatusCode.BadRequest);

            category = mapper.Map(request, category);
            categoryRepository.Update(category);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
                return ServiceResult.Fail("Category not found", HttpStatusCode.NotFound);
            categoryRepository.Delete(category);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
