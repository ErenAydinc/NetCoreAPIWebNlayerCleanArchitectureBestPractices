using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Categories.Dto
{
    public record CategoryWithProductsDto(int Id, string Name, List<Product> Products);
}
