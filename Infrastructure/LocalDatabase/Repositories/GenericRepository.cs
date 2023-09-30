using Application.Repositiories;
using Domain.Common;
using Infrastructure.LocalDatabase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.LocalDatabase.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DataContext _dataContext;

        #region Constructor / Setup

        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #endregion

        public async Task Create(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteMany(IEnumerable<T> entities)
        {
            _dataContext.Set<T>().RemoveRange(entities);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id) ?? throw new Exception();
        }

        public IQueryable<T> GetAll()
        {
            return _dataContext.Set<T>();
        }

        public async Task Update(T entity)
        {
            _dataContext.Set<T>().Update(entity);
            await _dataContext.SaveChangesAsync();
        }
    }
}
