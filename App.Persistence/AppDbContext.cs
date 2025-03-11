using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Persistence
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
