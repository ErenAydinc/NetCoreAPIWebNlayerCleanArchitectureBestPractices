using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category,int>(context), ICategoryRepository
    {
        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await Context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            return Context.Categories.Include(x => x.Products).AsQueryable();
        }
    }
}
