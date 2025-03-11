using App.Application.Contracts.Persistence;

namespace App.Persistence
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangeAsync() => await context.SaveChangesAsync();
    }
}
