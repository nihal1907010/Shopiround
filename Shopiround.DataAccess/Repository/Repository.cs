using Microsoft.EntityFrameworkCore;
using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext applicationDbContext;
        internal DbSet<T> databaseSet;
        public Repository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            databaseSet = applicationDbContext.Set<T>();
        }
        public void Add(T entity)
        {
            databaseSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = databaseSet;
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> values = databaseSet;
            values = values.Where(filter);
            if (string.IsNullOrEmpty(includeProperties) == false)
            {
                foreach (var prop in
                    includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    values = values.Include(prop);
                }
            }
            return values.FirstOrDefault();
        }
    }
}
