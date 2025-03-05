using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    //Aşağıdaki kullanım yerine primary constructor yazabiliriz
    public record ProductDto(int Id,string Name,decimal Price,int Stock,int CategoryId);




    ////record nesneler arasında eşitlik kontrolü yaparken kolaylık sağlar normalde iki class arasında eşitlik kontrolü (product1==product2) yapılamaz fals döner ama record da yapılabilir
    //public record ProductDto
    //{
    //    //liste dönen dto da veriye müdehale edilip değişiklik yapılmaz bunun için de set yerine init kullanırız
    //    public int Id { get; init; }
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //    public int Stock { get; init; }
    //}
}
