using Microsoft.EntityFrameworkCore;
using Shopiround.DataAccess.Data;
using Shopiround.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
