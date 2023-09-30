using Domain.Common;

namespace Application.Repositiories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> Get(int id);
        IQueryable<T> GetAll();
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeleteMany(IEnumerable<T> entities);
    }
}
