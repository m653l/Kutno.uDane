using Domain.Aggregates;

namespace Application.Repositiories
{
    public interface IMainRepository
    {
        Task<MainBeaver> Get(int id);
        IQueryable<MainBeaver> GetData();
        Task Create(MainBeaver beaver);
        Task Update(MainBeaver beaver);
    }
}
