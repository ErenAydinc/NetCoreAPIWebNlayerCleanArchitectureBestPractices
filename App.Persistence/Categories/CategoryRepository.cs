﻿using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await Context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetCategoryWithProducts()
        {
            return await Context.Categories.Include(x => x.Products).ToListAsync();
        }
    }
}
