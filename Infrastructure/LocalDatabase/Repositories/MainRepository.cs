using Application.Repositiories;
using Domain.Aggregates;
using Infrastructure.LocalDatabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LocalDatabase.Repositories
{
    public class MainRepository : GenericRepository<MainBeaver>, IMainRepository
    {
        public MainRepository(DataContext dataContext) : base(dataContext) { }

        public new async Task<MainBeaver> Get(int id)
        {
            return await _dataContext.Set<MainBeaver>()
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception();
        }

        public IQueryable<MainBeaver?> GetData()
        {
            return _dataContext.MainBeavers
                .AsNoTracking()
                .AsSplitQuery();
        }

        public new async Task Create(MainBeaver beaver)
        {
            _dataContext.MainBeavers.Add(beaver);

            await _dataContext.SaveChangesAsync();
        }

        public new async Task Update(MainBeaver beaver)
        {
            _dataContext.Update(beaver);
            await _dataContext.SaveChangesAsync();
        }
    }
}
