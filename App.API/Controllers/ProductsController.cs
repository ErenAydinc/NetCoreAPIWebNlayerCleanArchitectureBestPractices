using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : CustomBaseController
    {

        /// <summary>
        /// Get=> liste ve ya obje döndüğümüzde
        /// Post=> create işlemi yaptığımızda
        /// Put=> update işlemi yaptığımızda
        /// Patch=> belirli bir varlığı güncelleyeceğimizde
        /// Delete=> silme işleminde
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());
        
        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber,int pageSize) => CreateActionResult(await productService.GetPagedAllList(pageNumber,pageSize));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest createProductRequest) => CreateActionResult(await productService.CreateAsync(createProductRequest));

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductRequest updateProductRequest) => CreateActionResult(await productService.UpdateAsync(updateProductRequest));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));

        //Eğer belirli varlıkları güncelleyeceksek patch kullanmak daha uygun olur ama put kullanmakta da her hangi bir sakınca yoktur
        [HttpPatch("stock")]
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest updateProductStockRequest) => CreateActionResult(await productService.UpdateStockAsync(updateProductStockRequest));

        //[HttpPut("stock")]
        //public async Task<IActionResult> UpdateStock(UpdateProductStockRequest updateProductStockRequest) => CreateActionResult(await productService.UpdateStockAsync(updateProductStockRequest));
    }
}
