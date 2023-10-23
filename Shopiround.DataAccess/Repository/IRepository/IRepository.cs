using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllCondition(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);

        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Update(T entity);
        void Remove(T entity);

    }

    
}
