using System.Reflection;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Burada oluşturulan configurasyon sınıfları direkt olarak alınıyor inherit ettiğimi sınıftan dolayı
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
