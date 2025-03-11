namespace App.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangeAsync()=>await context.SaveChangesAsync();
    }
}
