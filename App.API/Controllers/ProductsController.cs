using App.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());
        
        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber,int pageSize) => CreateActionResult(await productService.GetPagedAllList(pageNumber,pageSize));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest createProductRequest) => CreateActionResult(await productService.CreateAsync(createProductRequest));

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest updateProductRequest) => CreateActionResult(await productService.UpdateAsync(updateProductRequest));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
    }
}
