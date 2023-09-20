using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
    }
}
