using System.Reflection;
using App.Repositories.Categories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Burada oluşturulan configurasyon sınıfları direkt olarak alınıyor inherit ettiğimiz sınıftan dolayı
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
