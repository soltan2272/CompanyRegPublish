using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRepositoryLayer.Interfaces
{
    public interface IGenericRepository<T>  where T : class
    {
       public Task<IEnumerable<T>> GetAllAsync();
       public Task<T?> GetByIdAsync(Guid id);
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task<bool> SaveChangesAsync();
    }
}
